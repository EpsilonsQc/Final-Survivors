#if UNITY_EDITOR
using Final_Survivors.Player;
using Final_Survivors.Weapons;
using UnityEngine;

namespace Final_Survivors.Environment
{
    public class EditorUtils : MonoBehaviour
    {
        private PlayerManager playerManager;
        private PlayerController playerController;
        private SpawnManager spawnManager;
        private Weapon weaponScript;
        private int choiceIndex = -1;
        private string[] bossEliteNames = new string[] { "DB", "NB", "RB", "RB", "SB", "DE", "NE", "RE", "RE", "SE" };

        private void Awake()
        {
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent <SpawnManager>();
        }

        public void OnGUI()
        {
            string godModeState = playerManager.GetIsGodMode() ? "God Mode ON" : "God Mode OFF";

            if (GUI.Button(new Rect(50, 50, 100, 30), godModeState))
            {
                playerManager.SetInvulnerability(!playerManager.GetInvulnerability());
                playerManager.SetIsGodMode(!playerManager.GetIsGodMode());
            }

            if (GUI.Button(new Rect(50, 150, 110, 30), "Refill Timewarp"))
            {
                playerController.RefillTimeWarp();
            }

            // Spawn Boss & Elites
            choiceIndex = GUI.Toolbar(new Rect(50, 200, 350, 30), choiceIndex, bossEliteNames);

            if (GUI.Button(new Rect(50, 250, 110, 30), "PauseSpawn"))
            {
                spawnManager.PauseSpawn();
            }

            if (GUI.Button(new Rect(175, 250, 110, 30), "PlaySpawn"))
            {
                spawnManager.PlaySpawn();
            }

            if (GUI.Button(new Rect(50, 300, 110, 30), "+ Level"))
            {
                ++spawnManager.Level;
            }

            if (GUI.Button(new Rect(175, 300, 110, 30), "- Level"))
            {
                if (spawnManager.Level > 1)
                    --spawnManager.Level;
            }

            if (GUI.Button(new Rect(50, 350, 110, 30), "Refill AMMO"))
            {
                weaponScript = playerController.selectedWeapon.GetComponent<Weapon>();
                weaponScript.RefillAmmo();
            }

            if (GUI.Button(new Rect(50, 400, 110, 30), "Shotgun"))
            {
                playerController.RandomPickWeapon(WeaponType.SHOTGUN);
            }

            if (GUI.Button(new Rect(175, 400, 110, 30), "Sniper"))
            {
                playerController.RandomPickWeapon(WeaponType.SNIPER);
            }

            if (GUI.Button(new Rect(50, 450, 110, 30), "Machine"))
            {
                playerController.RandomPickWeapon(WeaponType.MACHINEGUN);
            }

            if (GUI.Button(new Rect(175, 450, 110, 30), "Rocket"))
            {
                playerController.RandomPickWeapon(WeaponType.ROCKET_LAUNCHER);
            }

            if (GUI.Button(new Rect(50, 500, 110, 30), "GodSwordDMG"))
            {
                playerController.GetSword().GetComponent<Sword>().NewDmg(50000);
            }

            if (GUI.Button(new Rect(175, 500, 110, 30), "MinusSwordDMG"))
            {
                playerController.GetSword().GetComponent<Sword>().NewDmg(600);
            }
        }

        private void Update()
        {
            if (choiceIndex != -1)
            {
                switch (choiceIndex)
                {
                    case 0:
                        spawnManager.SpawnBoss(0);
                        break;
                    case 1:
                        spawnManager.SpawnBoss(1);
                        break;
                    case 2:
                        spawnManager.SpawnBoss(2);
                        break;
                    case 3:
                        spawnManager.SpawnBoss(3);
                        break;
                    case 4:
                        spawnManager.SpawnBoss(4);
                        break;
                    case 5:
                        spawnManager.SpawnElite(0);
                        break;
                    case 6:
                        spawnManager.SpawnElite(1);
                        break;
                    case 7:
                        spawnManager.SpawnElite(2);
                        break;
                    case 8:
                        spawnManager.SpawnElite(3);
                        break;
                    case 9:
                        spawnManager.SpawnElite(4);
                        break;
                }

                choiceIndex = -1;
            } 
        }
    }
}
#endif
