using Final_Survivors.Core;
using Final_Survivors.Observer;
using Final_Survivors.Weapons;
using Final_Survivors.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Final_Survivors.Player
{
    public class PlayerController : Subject, UserInput.IPlayerActionsActions, UserInput.IPlayerMovementsActions
    {
        _PlayerMovementSM _playerMovementSM;
        PlayerManager playerManager;
        UserInput playerInputs;
        InputAction move;
        float dashRecast, timeWarpRecast;
        [SerializeField] public int dashNumber;
        [SerializeField] public int dashMax;
        [SerializeField] public float dashCD;
        [SerializeField] public float currentTimeWarp;
        [SerializeField] public float timeWarpMax;
        [SerializeField] public float timeWarpMin;
        [SerializeField] public float timeWarpRegenAmount;
        [SerializeField] public float timeWarpDecreaseAmount;
        public bool isInfiniteTimeWarp = false;
        private bool isUsingTimeWarp = false;
        public Dictionary<Events, bool> ActionTriggers { get; set; }
        private IKWeapons iKWeapons;
        public GameObject selectedWeapon;
        GameObject selectedFirearm;
        GameObject specialWeapon;
        [SerializeField] private GameObject pistol;
        [SerializeField] private GameObject sword;
        [SerializeField] private GameObject machineGun;
        [SerializeField] private GameObject shotgun;
        [SerializeField] private GameObject sniper;
        [SerializeField] private GameObject rocketLauncher;
        private Sword swordComponent;
        private Collider swordCollider;
        private Light mainLight, flashLight;
        private MeshCollider flCollider;
        private BoxCollider playerBoxCollider;
        private ParticleSystem[] timeWarpFx;
        private ParticleSystem[] dashFx;
        private Vector3 boxColliderCenter = new Vector3(0, 0.9f, 0);
        private Vector3 boxColliderSize = new Vector3(0.4f, 1.8f, 0.3f);
        private Vector3 boxColliderCenterDash = new Vector3(0, 0.2f, 0);
        private Vector3 boxColliderSizeDash = new Vector3(0.4f, 0.2f, 0.3f);
        private const float flashLightIntensity = 1.4f;
        private const float mainLightIntensity = 1f;
        private const float timeWarpDuration = 1f;
        private const float meleeDuration = 1f;
        private const float dashDuration = 0.9f;
        private float timerMelee = 0f;
        private ParticleSystem fxSword;

        void Awake()
        {
            mainLight = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>();
            flashLight = GameObject.FindGameObjectWithTag("FlashLight").GetComponent<Light>();
            flCollider = GameObject.FindGameObjectWithTag("FlashLight").GetComponentInChildren<MeshCollider>();
            playerBoxCollider = GetComponent<BoxCollider>();
            timeWarpFx = GameObject.FindGameObjectWithTag("TimeWarp").GetComponentsInChildren<ParticleSystem>();
            dashFx = GameObject.FindGameObjectWithTag("DashFx").GetComponentsInChildren<ParticleSystem>();
            fxSword = sword.GetComponentInChildren<ParticleSystem>();

            iKWeapons = GetComponent<IKWeapons>();
            Cursor.lockState = CursorLockMode.Confined;
            _playerMovementSM = GetComponent<_PlayerMovementSM>();
            playerManager = GetComponent<PlayerManager>();
            playerInputs = new UserInput();
            playerInputs.PlayerActions.SetCallbacks(this);
            playerInputs.PlayerMovements.SetCallbacks(this);
            playerInputs.PlayerActions.Enable();
            playerInputs.PlayerMovements.Enable();
            move = playerInputs.PlayerMovements.Move;

            swordComponent = sword.GetComponent<Sword>();
            swordCollider = sword.GetComponent<Collider>();
            swordCollider.enabled = false;
            timerMelee = meleeDuration;

            InitWeapons();
            specialWeapon = null;
            selectedWeapon = pistol;
            selectedFirearm = pistol;
            ActivateSelectedWeapon(pistol);

            ActionTriggers = new Dictionary<Events, bool>();
            SetupTriggers();
        }

        void Start()
        {
            NotifyObservers(Events.IDLE);
        }

        void Update()
        {
            if (!EnvironmentState.GetIsPause())
            {
                if (dashRecast > 0)
                {
                    dashRecast -= Time.deltaTime;
                }

                if (timeWarpRecast > 0)
                {
                    timeWarpRecast -= Time.deltaTime;
                }

                if (EnvironmentState.GetIsDay() && currentTimeWarp < timeWarpMax)
                {
                    TimeWarpRegen();
                }

                if (!EnvironmentState.GetIsDay() && currentTimeWarp > 0 && !isInfiniteTimeWarp)
                {
                    TimeWarpDecrease();
                }
            }
        }

        private void OnDisable()
        {
            playerInputs.PlayerActions.Disable();
            playerInputs.PlayerMovements.Disable();
        }

        private void InitWeapons()
        {
            pistol.SetActive(false);
            machineGun.SetActive(false);
            shotgun.SetActive(false);
            sniper.SetActive(false);
            rocketLauncher.SetActive(false);
            sword.SetActive(false);
        }

        public void EnableSwordCollider()
        {
            swordCollider.enabled = true;
        }

        public void DisableSwordCollider()
        {
            swordCollider.enabled = false;
            swordComponent.hasHit= false;
        }

        public GameObject GetSword()
        {
            return sword;
        }

        public void OnTbag(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ActivateSelectedWeapon(null);
                ActionTriggers[Events.TBAG] = true;
            }

            if (context.canceled)
            {
                ActionTriggers[Events.TBAG] = false;
                ActivateSelectedWeapon(selectedWeapon);
            }
        }

        public void OnMove(InputAction.CallbackContext context )
        {
            if (context.started)
            {
                NotifyObservers(Events.MOVE);
                InvokeRepeating(nameof(SetMoveDirection), 0, Time.deltaTime);
                ActionTriggers[Events.MOVE] = true;
            }

            if (context.canceled)
            {
                CancelInvoke(nameof(SetMoveDirection));
                ActionTriggers[Events.MOVE] = false;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.started && dashRecast <= 0 && !ActionTriggers[Events.TIME_WARP] && dashNumber > 0)
            {
                SwitchToDashMode(true);
                NotifyObservers(Events.DASH);
                NotifyObservers(Events.DASH_MINUS);
                ActionTriggers[Events.DASH] = true;
                StartCoroutine(nameof(DashDuration));
                StartCoroutine(nameof(DashCooldown));

                foreach (ParticleSystem particleSystem in dashFx)
                {
                    particleSystem.Play();
                }

                if (!playerManager.GetIsGodMode())
                    playerManager.SetInvulnerability(true);

                dashRecast = dashDuration + 0.1f;
                --dashNumber;
            }

            //if(context.performed)
            //{
            //    actionTriggers[Events.DASH] = false;
            //}
        }

        public void OnTimeWarp(InputAction.CallbackContext context)
        {
            if (context.started && timeWarpRecast <= 0 && !ActionTriggers[Events.TIME_WARP])
            {
                if (!isUsingTimeWarp)
                {
                    if (EnvironmentState.GetIsDay() && currentTimeWarp > timeWarpMin || !EnvironmentState.GetIsDay())
                        LaunchTimeWarp(true);
                }
            }
        }

        private void LaunchTimeWarp(bool byPlayer = false)
        {
            isUsingTimeWarp = true;

            NotifyObservers(Events.TIME_WARP);

            if (byPlayer)
            {
                ActivateSelectedWeapon(null);
                ActionTriggers[Events.TIME_WARP] = true;
            }

            foreach(ParticleSystem particleSystem in timeWarpFx)
            {
                particleSystem.Play();
            }

            StartCoroutine(SetDayNightTransition(EnvironmentState.GetIsDay()));
            StartCoroutine(nameof(TimeWarpDuration));
            timeWarpRecast = timeWarpDuration + 0.25f;
        }

        public void OnChangeWeapon(InputAction.CallbackContext context)
        {
            if (context.started && specialWeapon != null && !ActionTriggers[Events.TIME_WARP])
            {
                switch(selectedFirearm.name)
                {
                    case "Pistol": selectedFirearm = specialWeapon; ActivateSelectedWeapon(specialWeapon); break;
                    default: selectedFirearm = pistol; ActivateSelectedWeapon(pistol); break;
                }
            }
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started && !ActionTriggers[Events.TIME_WARP])
            {
                NotifyObservers(Events.SHOOT);
                ActivateSelectedWeapon(selectedFirearm);
                ActionTriggers[Events.SHOOT] = true;
            }

            if (context.canceled)
            {
                ActionTriggers[Events.SHOOT] = false;
            }
        }

        public void OnMelee(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ActionTriggers[Events.MELEE] = true;
            }

            if (ActionTriggers[Events.MELEE] && !ActionTriggers[Events.TIME_WARP])
            {
                if (timerMelee >= meleeDuration)
                {
                    ActivateSelectedWeapon(sword);
                    NotifyObservers(Events.MELEE);
                    timerMelee = 0f;
                    StartCoroutine(nameof(MeleeDuration));
                }
            }

            if (context.canceled)
            {
                ActionTriggers[Events.MELEE] = false;
            }
        }

        public void ActivateFxSword()
        {
            fxSword.Play();
        }

        public void DeactivateFxSword()
        {
            fxSword.Stop();
        }

        private void ActivateSelectedWeapon(GameObject ActivatedWeapon = null)
        {
            if (selectedWeapon != ActivatedWeapon)
                selectedWeapon.SetActive(false);

            if (ActivatedWeapon != null)
            {
                selectedWeapon = ActivatedWeapon;
                selectedWeapon.SetActive(true);

                if (selectedWeapon.name != "Sword")
                {
                    selectedFirearm = ActivatedWeapon;

                    Transform leftHand = ActivatedWeapon.transform.Find("LeftHandPosition");
                    Transform rightHand = ActivatedWeapon.transform.Find("RightHandPosition");

                    iKWeapons.SetIK(leftHand, rightHand);
                    iKWeapons.SetIKActive(true);
                }
                else
                {
                    DisableSwordCollider();
                    iKWeapons.SetIKActive(false);
                }

                NotifySelectedWeapon();
            }
            else
            {
                iKWeapons.SetIKActive(false);
            }
        }

        private void NotifySelectedWeapon()
        {
            switch(selectedWeapon.name)
            {
                case "Pistol":
                    NotifyObservers(Events.PISTOL);
                    break;

                case "Sword":
                    NotifyObservers(Events.SWORD);
                    break;

                case "MachineGun":
                    NotifyObservers(Events.MACHINE_GUN);
                    break;

                case "SniperRifle":
                    NotifyObservers(Events.SNIPER);
                    break;

                case "Shotgun":
                    NotifyObservers(Events.SHOTGUN);
                    break;

                case "RocketLauncher":
                    NotifyObservers(Events.ROCKET_LAUNCHER);
                    break;
            }
        }

        private IEnumerator SetDayNightTransition(bool value)
        {
            float timer = Time.deltaTime / timeWarpDuration;

            for (float intensity = 1; intensity >= 0; intensity -= timer)
            {
                mainLight.intensity = value ? intensity : 1 - intensity;
                yield return new WaitForSeconds(timer);
            }
        }

        private IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashCD);

            if (dashNumber < dashMax)
            {
                ++dashNumber;
                NotifyObservers(Events.DASH_PLUS);
            } 
        }

        public void RefillTimeWarp()
        {
            currentTimeWarp = timeWarpMax;
            NotifyObservers(Events.TIME_WARP_UPDATE);
        }

        private void TimeWarpRegen()
        {
            currentTimeWarp += Time.deltaTime * timeWarpRegenAmount;

            if (currentTimeWarp >= timeWarpMax)
                currentTimeWarp = timeWarpMax;

            NotifyObservers(Events.TIME_WARP_UPDATE);
        }

        private void TimeWarpDecrease()
        {
            currentTimeWarp -= Time.deltaTime * timeWarpDecreaseAmount;

            if (currentTimeWarp <= 0f)
            {
                currentTimeWarp = 0f;

                if (!isUsingTimeWarp)
                    LaunchTimeWarp(false);
            }

            NotifyObservers(Events.TIME_WARP_UPDATE);
        }

        private IEnumerator FlashligthFlicker()
        {
            flashLight.intensity = Mathf.Lerp(0, flashLightIntensity, 0.125f);
            yield return new WaitForSecondsRealtime(0.125f);
            flashLight.intensity = Mathf.Lerp(flashLightIntensity, 0, 0.125f);
            yield return new WaitForSecondsRealtime(0.125f);
            flashLight.intensity = Mathf.Lerp(0, flashLightIntensity, 0.25f);
            yield return new WaitForSecondsRealtime(0.25f);
            flashLight.intensity = Mathf.Lerp(flashLightIntensity, 0, 0.5f);
            yield return new WaitForSecondsRealtime(0.5f);
            flashLight.intensity = Mathf.Lerp(flashLightIntensity, 0, 0.125f);
            yield return new WaitForSecondsRealtime(0.125f);
            flashLight.intensity = flashLightIntensity;
        }

        public void RandomPickWeapon(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.MACHINEGUN:
                    specialWeapon = machineGun;
                    break;
                case WeaponType.SHOTGUN:
                    specialWeapon = shotgun;
                    break;
                case WeaponType.SNIPER:
                    specialWeapon = sniper;
                    break;
                case WeaponType.ROCKET_LAUNCHER:
                    specialWeapon = rocketLauncher;
                    break;
            }
            
            ActivateSelectedWeapon(specialWeapon);
            NotifyObservers(Events.RELOAD);
            NotifyObservers(Events.AMMO);
        }

        private void SwitchToDashMode(bool value)
        {
            if (value)
            {
                playerBoxCollider.center = boxColliderCenterDash;
                playerBoxCollider.size = boxColliderSizeDash;
            }
            else
            {
                playerBoxCollider.center = boxColliderCenter;
                playerBoxCollider.size = boxColliderSize;
            }
        }

        private void SetupTriggers()
        {
            ActionTriggers.Add(Events.MOVE, false);
            ActionTriggers.Add(Events.DASH, false);
            ActionTriggers.Add(Events.SHOOT, false);
            ActionTriggers.Add(Events.MELEE, false);
            ActionTriggers.Add(Events.TIME_WARP, false);
            ActionTriggers.Add(Events.TBAG, false);
        }

        void SetMoveDirection()
        {
            _playerMovementSM.moveInputValue = move.ReadValue<Vector2>();
        }

        public void OnMouseCursor(InputAction.CallbackContext context)
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Confined; // Confine the cursor
                //Cursor.visible = false; // Make the cursor invisible
            }
            else if (Cursor.lockState == CursorLockMode.Confined)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                //Cursor.visible = true; // Make the cursor visible
            }
        }

        IEnumerator DashDuration()
        {
            yield return new WaitForSeconds(dashDuration);

            foreach (ParticleSystem particleSystem in dashFx)
            {
                particleSystem.Stop();
            }

            SwitchToDashMode(false);

            if (!playerManager.GetIsGodMode())
                playerManager.SetInvulnerability(false);

            ActionTriggers[Events.DASH] = false;
        }

        IEnumerator MeleeDuration() 
        {
            while (timerMelee < meleeDuration)
            {
                timerMelee += Time.deltaTime;
                yield return null;
            }

            DisableSwordCollider();
        }

        IEnumerator TimeWarpDuration()
        {
            if (EnvironmentState.GetIsDay())
            {
                NotifyObservers(Events.TIME_WARP_DAY_TO_NIGHT);
            }

            yield return new WaitForSeconds(timeWarpDuration);

            ActivateSelectedWeapon(selectedWeapon);
            
            foreach (ParticleSystem particleSystem in timeWarpFx)
            {
                particleSystem.Stop();
            }

            if (EnvironmentState.GetIsDay())
            {
                StartCoroutine(FlashligthFlicker());
            }
            else
            {
                flashLight.intensity = 0;
                NotifyObservers(Events.TIME_WARP_NIGHT_TO_DAY);
            }

            EnvironmentState.SetIsDay(!EnvironmentState.GetIsDay());
            flCollider.enabled = !EnvironmentState.GetIsDay();
            ActionTriggers[Events.TIME_WARP] = false;
            isUsingTimeWarp = false;
        }
    }
}
