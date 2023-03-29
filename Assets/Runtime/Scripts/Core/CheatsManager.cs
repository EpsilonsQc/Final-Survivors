using UnityEngine;
using UnityEngine.InputSystem;
using Final_Survivors.Player;
using Final_Survivors.Weapons;
using TMPro;
using Final_Survivors.Environment;

namespace Final_Survivors.Core
{
    public class CheatsManager : MonoBehaviour
    {
        [Header("Cheats Menu")]
        [SerializeField] private GameObject cheatsMenu;

        [Header("God Mode")]
        [SerializeField] private TextMeshProUGUI godModeState;

        [Header("Infinite Time Warp")]
        [SerializeField] private TextMeshProUGUI infiniteTimeWarpState;

        [Header("References")]
        [SerializeField] private GameObject Background;
        [SerializeField] private GameObject CheatsText;
        [SerializeField] private GameObject Buttons;
        [SerializeField] private CursorManager cursorManager; // Reference to CursorManager.cs
        [SerializeField] private GameObject pauseMenu;

        // TELEPORT POSITIONS
        private Vector3 southWest = new Vector3(-35.1899986f, -5.0f, -39.9900017f);
        private Vector3 southEast = new Vector3(20f, -5f, -52.3600006f);
        private Vector3 northWest = new Vector3(-46.4749985f, -5.0f, 57.3720016f);
        private Vector3 northEast = new Vector3(12.9200001f, -5.0f, 56.9900017f);

        // CHEATS RELATED STUFF
        private GameObject player;
        private PlayerManager playerManager;
        private PlayerController playerController;
        private SpawnManager spawnManager;
        private Weapon weaponScript;
        
        // Player Input
        private PlayerInput playerInput;

        private void OnEnable()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.actions["Toggle Cheats Menu"].performed += OnShowOrHideCheatsMenu;
        }

        private void OnDisable()
        {
            playerInput.actions["Toggle Cheats Menu"].performed -= OnShowOrHideCheatsMenu;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent <SpawnManager>();
            playerInput = GetComponent<PlayerInput>();
        }

        public void OnShowOrHideCheatsMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if(!EnvironmentState.GetIsIntroduction())
                {
                    if (cheatsMenu.activeSelf == false)
                    {
                        if (pauseMenu.activeSelf == false) // prevent opening the in-game cheats menu while the pause menu is open
                        {
                            OpenCheatsMenu();
                        }
                    }
                    else if (cheatsMenu.activeSelf == true)
                    {
                        CloseCheatsMenu();
                    }
                }
            }
        }

        // OPEN/CLOSE CHEAT MENU
        private void OpenCheatsMenu()
        {
            // Pause the game
            Time.timeScale = 0f; // Pause the game
            EnvironmentState.SetIsPause(true); // Pause the game (for the environment)
            cursorManager.IsPausedCursor = true;

            // Show the cheats menu
            cheatsMenu.SetActive(true);

            // Hide some UI elements
            Background.SetActive(false);
            CheatsText.SetActive(false);
            Buttons.SetActive(false);
        }

        public void CloseCheatsMenu()
        {
            // Resume the game
            Time.timeScale = 1f; // Resume the game
            EnvironmentState.SetIsPause(false); // Resume the game (for the environment)
            cursorManager.IsPausedCursor = false;

            // Hide the cheats menu
            cheatsMenu.SetActive(false);

            // Restore/show some UI elements
            Background.SetActive(true);
            CheatsText.SetActive(true);
            Buttons.SetActive(true);
        }

        // DEV
        public void GodMode()
        {
            playerManager.SetInvulnerability(!playerManager.GetInvulnerability());
            playerManager.SetIsGodMode(!playerManager.GetIsGodMode());

            if (playerManager.GetIsGodMode())
            {
                godModeState.text = "God Mode ON";
            }
            else
            {
                godModeState.text = "God Mode OFF";
            }
        }

        // SPAWN
        public void PauseSpawn()
        {
            spawnManager.PauseSpawn();
        }

        public void ResumeSpawn()
        {
            spawnManager.PlaySpawn();
        }

        public void DecreaseDifficulty()
        {
            if (spawnManager.Level > 1)
            {
                --spawnManager.Level;
            }
        }

        public void IncreaseDifficulty()
        {
            ++spawnManager.Level;
        }

        // WEAPONS
        public void Shotgun()
        {
            playerController.RandomPickWeapon(WeaponType.SHOTGUN);
        }

        public void MachineGun()
        {
            playerController.RandomPickWeapon(WeaponType.MACHINEGUN);
        }

        public void Sniper()
        {
            playerController.RandomPickWeapon(WeaponType.SNIPER);
        }

        public void RocketLauncher()
        {
            playerController.RandomPickWeapon(WeaponType.ROCKET_LAUNCHER);
        }

        // SWORD
        public void MaxSwordDamage()
        {
            playerController.GetSword().GetComponent<Sword>().NewDmg(50000);
        }

        public void DefaultSwordDamage()
        {
            playerController.GetSword().GetComponent<Sword>().NewDmg(600);
        }

        // TELEPORT
        public void TP_SouthWest()
        {
            player.transform.position = southWest;
        }

        public void TP_SouthEast()
        {
            player.transform.position = southEast;
        }

        public void TP_NorthWest()
        {
            player.transform.position = northWest;
        }

        public void TP_NorthEast()
        {
            player.transform.position = northEast;
        }
        
        // REFILL
        public void AmmoRefill()
        {
            weaponScript = playerController.selectedWeapon.GetComponent<Weapon>();
            weaponScript.RefillAmmo();
        }

        public void TimewarpRefill()
        {
            playerController.RefillTimeWarp();
        }

        public void TimewarpInfinite()
        {
            if (!playerController.isInfiniteTimeWarp)
            {
                infiniteTimeWarpState.text = "Infinite ON";
                TimewarpRefill();
                playerController.isInfiniteTimeWarp = true;
            }
            else
            {
                infiniteTimeWarpState.text = "Infinite OFF";
                playerController.isInfiniteTimeWarp = false;
            }
        }

        // SPAWN BOSS
        public void DragonBoss()
        {
            spawnManager.SpawnBoss(0);
        }

        public void SpiderBoss()
        {
            spawnManager.SpawnBoss(4);
        }

        public void RazorBoss()
        {
            spawnManager.SpawnBoss(2);
        }

        public void NoseBoss()
        {
            spawnManager.SpawnBoss(1);
            
        }

        public void RobotBoss()
        {
            spawnManager.SpawnBoss(3);
            
        }

        // SPAWN ELITE
        public void DragonElite()
        {
            spawnManager.SpawnElite(0);
        }

        public void SpiderElite()
        {
            spawnManager.SpawnElite(4);
        }

        public void RazorElite()
        {
            spawnManager.SpawnElite(2);
        }

        public void NoseElite()
        {
            spawnManager.SpawnElite(1);
        }

        public void RobotElite()
        {
            spawnManager.SpawnElite(3);
        }
    }
}
