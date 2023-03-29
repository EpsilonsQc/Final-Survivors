using Final_Survivors.Audio;
using Final_Survivors.Player;
using Final_Survivors.Projectile;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class DragonBoss : Enemy
    {
        [SerializeField] private Transform mouth;
        [SerializeField] private AudioSource source;

        [Header("Breath Settings")]
        [SerializeField] private ParticleSystem flameBreathSystem;
        [SerializeField] private float breathDamage;
        [SerializeField] private float breathCooldown;
        [SerializeField] private float breathRange;
        [SerializeField] private float breathRotSpeed;
        [SerializeField] private AudioClip[] breathClip;
        private float breathCooldownTimer = 0f;

        [Header("Fireball Settings")]
        [SerializeField] private GameObject fireBallPrefab;
        [SerializeField] private float fireballDamage;
        [SerializeField] private float fireballSpeed;
        [SerializeField] private float fireBallRange;
        [SerializeField] private float fireballCooldown;
        private float fireballCooldownTimer = 0f;

        [Header("MeleeSettings")]
        [SerializeField] private float meleeDamage;
    
        public bool isBreathReady { get; set; }
        public float BreathRange { get { return breathRange; } }
        public float BreathDamage { get { return breathDamage; } }

        public bool isFireballReady { get; set; }
        public float FireballRange { get { return fireBallRange; } }
        public float FireballDamage { get { return fireballDamage; } }

        public ParticleSystem FlameBreathSystem { get { return flameBreathSystem; } }

        private void Awake()
        {
            isFireballReady = true;
            isBreathReady = true;
            Level = EnemyLevel.BOSS;
        }

        private void Update()
        {
            if(!IsDead)
                RotateTowardsPlayer();
        }

        private void RotateTowardsPlayer()
        {                   
            Vector3 newForward = (playerTransform.position - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, newForward, breathRotSpeed * Time.deltaTime);
        }

        public void MeleeAttack()
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {    
                playerTransform.GetComponent<PlayerManager>().TakeDamage(meleeDamage);
            }
        }

        public void StopMeleeAttack()
        {
            isAttacking= false;
            animator.SetBool("isAttacking", false);
            AttackCooldown();
        }

        public void ShootFireball()
        {
            Vector3 dir = (playerTransform.position - mouth.position).normalized;
            GameObject go = Instantiate(fireBallPrefab);
            go.GetComponent<Fireball>().SetupFireball(fireballDamage, mouth.position, dir,fireballSpeed);
        }

        public void StopFireball()
        {
            isAttacking= false;
            isFireballReady= false;
            StartCoroutine(nameof(CooldownFireball));
        }

        private IEnumerator CooldownFireball()
        {
            while (fireballCooldownTimer < fireballCooldown)
            {
                fireballCooldownTimer += Time.deltaTime;

                yield return null;
            }

            fireballCooldownTimer = 0f;
            isFireballReady= true;
        }

        public void PlayBreathSound()
        {
            SoundManager.PlaySound(ref breathClip, source, source.volume / 2);
        }

        public void DoFireBreath()
        {
            flameBreathSystem.Play();
        }

        public void StopBreath()
        {
            flameBreathSystem.Stop();
            isAttacking= false;
            isBreathReady= false;
            StartCoroutine(nameof(BreathCooldown));
        }

        public IEnumerator BreathCooldown()
        {
            while(breathCooldownTimer < breathCooldown)
            {
                breathCooldownTimer += Time.deltaTime;

                yield return null;
            }

            breathCooldownTimer = 0f;
            isBreathReady= true;
        }
    }
}
