using UnityEngine;
using System.Collections;
using Cinemachine;
using Final_Survivors.Audio;
using Final_Survivors.Core;
using Final_Survivors.UI.GameOverMenu;
using Final_Survivors.Observer;
using UnityEngine.Rendering.PostProcessing;

namespace Final_Survivors.Player
{
    public class PlayerManager : Subject, IDamageModifier
    {
        [Header("Health")]
        [SerializeField] private float health;
        [SerializeField] private float healthMaxValue = 100f; // default value

        [Header("Shield")]
        [SerializeField] private float shieldValue;
        [SerializeField] private float shieldMaxValue = 100f; // default value

        [Header("Movement Force")]
        [SerializeField] private float force;
        [SerializeField] private float dashForce;
        [SerializeField] private float speedModifier;
        [SerializeField] private float slowTimer;

        [Header("UI reference")]
        [SerializeField] private GameObject menuManager;

        // Audio
        AudioSource audioSource;
        SoundBank soundBank;

        // Camera
        private CinemachineVirtualCamera vCam;
        private Vector3 cursorPosition;

        public Rigidbody rb { get; private set; }
        [SerializeField] private bool isGodMode = false;
        private bool isInvulnerable = false;
        private ParticleSystem walkFx;

        public int cells = 0;

        //PostProcess
        private PostProcessVolume volume;
        private GameObject postProcess;
        private Vignette vignette;
        [SerializeField] ParticleSystem slowFx;

        private bool isFlashRunning;

        private int takenDamage = 0;
        private Events damageModifierEvt = Events.NORMAL_DMG;
        public int TakenDamage { get { return takenDamage; } private set { takenDamage = value; } }
        public Events DamageModifierEvt { get { return damageModifierEvt; } private set { damageModifierEvt = value; } }

        // Getters and Setters
        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public float HealthMaxValue
        {
            get { return healthMaxValue; }
            set { healthMaxValue = value; }
        }

        public float ShieldValue
        {
            get { return shieldValue; }
            set 
            { 
                shieldValue = value;
                NotifyObservers(Events.SHIELD);
            }
        }

        public float ShieldMaxValue
        {
            get { return shieldMaxValue; }
            set { shieldMaxValue = value; }
        }

        void Awake()
        {
            health = healthMaxValue;
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            soundBank = FindObjectOfType<SoundBank>();
            postProcess = GameObject.FindGameObjectWithTag("postProcess");
            volume = postProcess.GetComponent<PostProcessVolume>();
            volume.profile.TryGetSettings(out vignette);

            walkFx = GameObject.FindGameObjectWithTag("WalkFx").GetComponent<ParticleSystem>();
            vCam = GameObject.FindGameObjectWithTag("Vcam").GetComponent<CinemachineVirtualCamera>();
            vCam.Follow = transform;
            vCam.LookAt = transform;

            NotifyObservers(Events.INIT);
        }

        public void SetInvulnerability(bool value)
        {
            isInvulnerable = value;
        }

        public bool GetInvulnerability()
        {
            return isInvulnerable;
        }

        public void SetIsGodMode(bool value)
        {
            isGodMode = value;
        }

        public bool GetIsGodMode()
        {
            return isGodMode;
        }

        /***** USED BY ANIMATIONS *****/
        public void PlaySoundWalk()
        {
            SoundManager.PlaySound(ref soundBank.walk, audioSource, audioSource.volume / 3);
        }

        public void PlaySoundDash()
        {
            SoundManager.PlaySound(ref soundBank.dash, audioSource, audioSource.volume / 3);
        }

        public void PlaySoundCac()
        {
            SoundManager.PlaySound(ref soundBank.laserSword, audioSource, audioSource.volume / 3);
        }

        public void PlaySoundRangedAttack()
        {
            SoundManager.PlaySound(ref soundBank.submachineGun, audioSource, audioSource.volume / 3);
        }

        public void PlaySoundTimeWarp()
        {
            SoundManager.PlaySound(ref soundBank.timeWarp, audioSource, audioSource.volume / 3);
        }
        /**** USED BY ANIMATIONS ****/

        public void Move(Vector2 input)
        {
            RotateToMouse();
            Vector3 direction = new Vector3(input.x, 0, input.y);
            rb.AddForce(direction * force * speedModifier, ForceMode.Force);
        }

        public void RotateToMouse()
        {
            cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, Vector3.Distance(Camera.main.transform.position, transform.position)));
            cursorPosition = new Vector3(cursorPosition.x, transform.position.y, cursorPosition.z);
            transform.forward = (cursorPosition - transform.position).normalized;
        }

        public void Dash(Vector2 input)
        {
            SetWalkFx(0);

            if (input == Vector2.zero)
            {
                rb.AddForce(-rb.transform.forward * dashForce * speedModifier, ForceMode.Force);
            }
            else
            {
                rb.AddForce(new Vector3(input.x, 0, input.y) * dashForce * speedModifier, ForceMode.Force);
            }

            RotateToMouse();
        }

        public void TakeDamage(float damage)
        {
            if (!isInvulnerable)
            {
                TakenDamage = DamageModifier((int)damage);

                if (shieldValue >= TakenDamage)
                {
                    shieldValue -= TakenDamage;
                    vignette.color.value = Color.blue;
                    vignette.intensity.value = 0.58f;
                    if (!isFlashRunning)
                    {
                        isFlashRunning = true;
                        StartCoroutine(nameof(Flash));
                    }   
                }
                else
                {
                    TakenDamage -= (int)shieldValue;
                    shieldValue = 0;

                    if (health > TakenDamage)
                    {
                        health -= TakenDamage;
                        float healthValue = health / healthMaxValue;
                        vignette.color.value = Color.red;
                        vignette.intensity.value = Mathf.Lerp(0.72f, 0.52f, healthValue);
                    }
                    else
                    {
                        health = 0;
                        TakenDamage = 0;
                        menuManager.GetComponent<GameOverMenu>().GameOver();
                    }
                }

                NotifyObservers(Events.TAKE_DAMAGE);
                NotifyObservers(DamageModifierEvt);
            }
        }

        public int DamageModifier(int value)
        {
            int newDmg;
            int percentDmg = Random.Range(90, 111);
            DamageModifierEvt = Events.NORMAL_DMG;
            int rand = Random.Range(0, 101);

            if (rand == 0) // Miss
            {
                percentDmg = 0;
                DamageModifierEvt = Events.MISS;
            }
            else if (rand > 0 && rand < 8) // Low dmg
            {
                percentDmg = Random.Range(50, 76);
                DamageModifierEvt = Events.LOW_DMG;
            }
            else if (rand >= 8 && rand < 15) // Crit dmg
            {
                percentDmg = Random.Range(150, 201);
                DamageModifierEvt = Events.CRIT_DMG;
            }

            if (percentDmg > 0)
                newDmg = value * percentDmg / 100;
            else
                newDmg = 0;

            return newDmg;
        }

        public void SetSlow(float slowAmount, float duration)
        {
            SetSlowFx(true);
            speedModifier = slowAmount;
            slowTimer = duration;
            Debug.Log(speedModifier * force);
            InvokeRepeating(nameof(SlowTimer), 0, Time.fixedDeltaTime);
        }

        private void SlowTimer()
        {
            slowTimer -= Time.fixedDeltaTime;
            if (slowTimer <= 0)
            {
                speedModifier = 1;
                SetSlowFx(false);
                CancelInvoke(nameof(SlowTimer));
            }
        }

        public void SetWalkFx(int value = -1)
        {
            if (value == 1)
            {
                if (!walkFx.isPlaying)
                    walkFx.Play();
            }
            else if (value == 0)
                walkFx.Stop();
        }

        private void SetSlowFx(bool value)
        {
            if (value)
            {
                if (!slowFx.isPlaying)
                    slowFx.Play();
            }
            else
                slowFx.Stop();
        }

        IEnumerator Flash()
        {
            yield return new WaitForSeconds(1);
            vignette.color.value = Color.black;
            vignette.intensity.value = 0.55f;
            isFlashRunning = false;
        }
    }
}
