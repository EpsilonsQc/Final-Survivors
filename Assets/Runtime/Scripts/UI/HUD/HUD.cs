using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Final_Survivors.Player;
using Final_Survivors.Core;
using Final_Survivors.Observer;
using Final_Survivors.Weapons;

namespace Final_Survivors.UI.HUD
{
    public class HUD : MonoBehaviour, IObserver
    {
        [Header("PLAYER")]
        private Subject _playerManagerSubject;
        private Subject _playerControllerSubject;
        private PlayerManager _playerManager;
        private PlayerController _playerController;

        [Header("WEAPON")]
        [SerializeField] private Subject[] _weaponSubjects;
        [SerializeField] private Weapon[] weapons;
        private Weapon selectedWeapon;

        [Header("HEALTH")]
        private Slider healthBar; 
        private TextMeshProUGUI currentHealth;
        private TextMeshProUGUI maxHealth;

        [Header("SHIELD")]
        private Slider shieldBar;
        private TextMeshProUGUI currentShield;
        private TextMeshProUGUI maxShield;

        [Header("TIMEWARP")]
        private Slider timeWarpBar;
        private TextMeshProUGUI currentTimeWarp;
        private TextMeshProUGUI maxTimeWarp;

        [Header("AMMOS")]
        private TextMeshProUGUI ammosCount;
        private TextMeshProUGUI slash;
        private TextMeshProUGUI ammosCapacity;

        [Header("WEAPONS")]
        [SerializeField] private GameObject[] weaponsThumbs;

        [Header("SCORE")]
        private Subject _scoreSubject;
        private ScoreManager scoreScript;
        private TextMeshProUGUI score;

        [Header("DASH")]
        private Image[] dash;
        [SerializeField] private int dashCount = 3;

        [Header("SPAWN")]
        private Subject _spawnSubject;
        private TextMeshProUGUI spawn;
        [SerializeField] private int waitDisapear = 3;

        [Header("SPECIALBUFF")]
        [SerializeField] private Subject _buffSubject;
        private TextMeshProUGUI buff;
        [SerializeField] private int waitDisapearBuff = 8;

        private void Awake()
        {
            _playerManagerSubject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            _playerControllerSubject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _scoreSubject = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            scoreScript = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            _spawnSubject = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();

            // HEALTH
            healthBar = GameObject.Find("HUD/Health").GetComponent<Slider>();
            currentHealth = GameObject.Find("HUD/Health/Ressources/Current (Text)").GetComponent<TextMeshProUGUI>();
            maxHealth = GameObject.Find("HUD/Health/Ressources/Max (Text)").GetComponent<TextMeshProUGUI>();

            // SHIELD
            shieldBar = GameObject.Find("HUD/Shield").GetComponent<Slider>();
            currentShield = GameObject.Find("HUD/Shield/Ressources/Current (Text)").GetComponent<TextMeshProUGUI>();
            maxShield = GameObject.Find("HUD/Shield/Ressources/Max (Text)").GetComponent<TextMeshProUGUI>();

            // AMMOS
            ammosCount = GameObject.Find("HUD/Ammos/AmmosCount (Text)").GetComponent<TextMeshProUGUI>();
            ammosCount.outlineWidth = 0.1f; // Set the outline width
            ammosCount.outlineColor = Color.black; // Set the outline color
            slash = GameObject.Find("HUD/Ammos/Slash (Text)").GetComponent<TextMeshProUGUI>();
            ammosCapacity = GameObject.Find("HUD/Ammos/AmmosCapacity (Text)").GetComponent<TextMeshProUGUI>();
            ammosCapacity.outlineWidth = 0.1f; // Set the outline width
            ammosCapacity.outlineColor = Color.black; // Set the outline color

            // SCORE
            score = GameObject.Find("HUD/Score (Text)").GetComponent<TextMeshProUGUI>();
            score.outlineWidth = 0.1f; // Set the outline width
            score.outlineColor = Color.black; // Set the outline color

            // DASH
            dash = GameObject.Find("HUD/Dash").transform.GetComponentsInChildren<Image>();

            // TIMEWARP
            timeWarpBar = GameObject.Find("HUD/TimeWarp").GetComponent<Slider>();
            currentTimeWarp = GameObject.Find("HUD/TimeWarp/Ressources/Current (Text)").GetComponent<TextMeshProUGUI>();
            maxTimeWarp = GameObject.Find("HUD/TimeWarp/Ressources/Max (Text)").GetComponent<TextMeshProUGUI>();

            //SPAWN INFO
            spawn = GameObject.Find("HUD/Spawn (Text)").GetComponent<TextMeshProUGUI>();
            spawn.outlineWidth = 0.1f; // Set the outline width
            spawn.outlineColor = Color.black;

            //BUFF INFO
            buff = GameObject.Find("HUD/Buff (Text)").GetComponent<TextMeshProUGUI>();
            buff.outlineWidth = 0.1f; // Set the outline width
            buff.outlineColor = Color.black;
        }

        private void OnEnable()
        {
            foreach (Subject subject in _weaponSubjects)
            {
                subject.AddObserver(this);
            }

            _buffSubject.AddObserver(this);
            _spawnSubject.AddObserver(this);
            _scoreSubject.AddObserver(this);
            _playerManagerSubject.AddObserver(this);
            _playerControllerSubject.AddObserver(this);
        }

        private void OnDisable()
        {
            foreach (Subject subject in _weaponSubjects)
            {
                subject.RemoveObserver(this);
            }

            _buffSubject.RemoveObserver(this);
            _spawnSubject.RemoveObserver(this);
            _scoreSubject.RemoveObserver(this);
            _playerManagerSubject.RemoveObserver(this);
            _playerControllerSubject.RemoveObserver(this);
        }

        public void OnNotify(Events action)
        {
            if (action == Events.AMMO)
            {
                int ammoCount = (int)selectedWeapon.GetAmmoCount();
                ammosCount.text = ammoCount.ToString();

                slash.text = "/";

                int ammoCapacity = (int)selectedWeapon.GetAmmoCapacity();
                ammosCapacity.text = ammoCapacity.ToString();
            }
            
            if (action == Events.PISTOL)
            {
                SetSelectedWeaponWithName("Pistol");
                ammosCount.text = "\u221E"; // Infinity symbol
                slash.text = "";
                ammosCapacity.text = "";
                ThumbsManagement();
            }
            
            if (action == Events.SWORD)
            {
                SetSelectedWeaponWithName("Sword");
                ammosCount.text = "\u221E"; // Infinity symbol
                slash.text = "";
                ammosCapacity.text = "";
                ThumbsManagement();
            }
            
            if (action == Events.MACHINE_GUN)
            {
                SetSelectedWeaponWithName("MachineGun");
                int ammoCount = (int)selectedWeapon.GetAmmoCount();
                ammosCount.text = ammoCount.ToString();

                slash.text = "/";

                int ammoCapacity = (int)selectedWeapon.GetAmmoCapacity();
                ammosCapacity.text = ammoCapacity.ToString();
                ThumbsManagement();
            }
            
            if (action == Events.SHOTGUN)
            {
                SetSelectedWeaponWithName("Shotgun");
                int ammoCount = (int)selectedWeapon.GetAmmoCount();
                ammosCount.text = ammoCount.ToString();

                slash.text = "/";

                int ammoCapacity = (int)selectedWeapon.GetAmmoCapacity();
                ammosCapacity.text = ammoCapacity.ToString();
                ThumbsManagement();
            }
            
            if (action == Events.SNIPER)
            {
                SetSelectedWeaponWithName("SniperRifle");
                int ammoCount = (int)selectedWeapon.GetAmmoCount();
                ammosCount.text = ammoCount.ToString();

                slash.text = "/";

                int ammoCapacity = (int)selectedWeapon.GetAmmoCapacity();
                ammosCapacity.text = ammoCapacity.ToString();
                ThumbsManagement();
            }
            
            if (action == Events.ROCKET_LAUNCHER)
            {
                SetSelectedWeaponWithName("RocketLauncher");
                int ammoCount = (int)selectedWeapon.GetAmmoCount();
                ammosCount.text = ammoCount.ToString();

                slash.text = "/";

                int ammoCapacity = (int)selectedWeapon.GetAmmoCapacity();
                ammosCapacity.text = ammoCapacity.ToString();
                ThumbsManagement();
            }

            if (action == Events.SCORE)
            {
                score.text = "SCORE : " + scoreScript.GetScore().ToString();
            }

            if (action == Events.TAKE_DAMAGE || action == Events.INIT || action == Events.SHIELD || action == Events.HEALTH)
            {
                // Update Health
                healthBar.value = (int)_playerManager.Health;

                int healthValue = (int)_playerManager.Health;
                currentHealth.text = healthValue.ToString();

                int healthMaxValue = (int)_playerManager.HealthMaxValue;
                maxHealth.text = healthMaxValue.ToString();

                // Update Shield
                shieldBar.value = (int)_playerManager.ShieldValue;

                int shieldValue = (int)_playerManager.ShieldValue;
                currentShield.text = shieldValue.ToString();

                int shieldMaxValue = (int)_playerManager.ShieldMaxValue;
                maxShield.text = shieldMaxValue.ToString();
            }

            if (action == Events.DASH_PLUS)
            {
                AddDash();
            }

            if (action == Events.DASH_MINUS)
            {
                RemoveDash();
            }

            if (action == Events.TIME_WARP_UPDATE || action == Events.INIT)
            {
                // Update TimeWarp
                timeWarpBar.value = (int)_playerController.currentTimeWarp;

                int timeWarpValue = (int)_playerController.currentTimeWarp;
                currentTimeWarp.text = timeWarpValue.ToString();

                int timeWarpMaxValue = (int)_playerController.timeWarpMax;
                maxTimeWarp.text = timeWarpMaxValue.ToString();
            }

            if (action == Events.SPAWNELITE)
            {
                spawn.enabled = true;
                spawn.color = Color.yellow;
                spawn.text = "An ELITE has spawned! ";
                StartCoroutine(nameof(WaitToDisapear));
            }
            
            if (action == Events.SPAWNBOSS)
            {
                spawn.enabled = true;
                spawn.color = Color.red;
                spawn.text = "A BOSS has spawned! ";
                StartCoroutine(nameof(WaitToDisapear));
            }

            if (action == Events.SPECIALCRATE_PICKUP)
            {
                buff.enabled = true;
                buff.color = Color.green;
                buff.text = "Your max health has INCREASED! ";
                StartCoroutine(nameof(WaitToDisapearBuff));
            }
        }

        IEnumerator WaitToDisapear()
        {
            yield return new WaitForSeconds(waitDisapear);
            spawn.enabled = false;
        }
        IEnumerator WaitToDisapearBuff()
        {
            yield return new WaitForSeconds(waitDisapearBuff);
            buff.enabled = false;
        }

        private void ThumbsManagement()
        {
            foreach (GameObject go in weaponsThumbs)
            {
                if (go.name == selectedWeapon.name)
                {
                    go.SetActive(true);
                }
                else if (go.name == "Background")
                {
                    continue;
                }
                else
                {
                    go.SetActive(false);
                }
            }
        }

        private void AddDash()
        {
            ++dashCount;
            DisplayDash();
        }

        private void RemoveDash()
        {
            --dashCount;
            DisplayDash();
        }

        private void DisplayDash()
        {
            switch (dashCount)
            {
                case 0:
                    dash[0].color = Color.red;
                    dash[1].color = Color.red;
                    dash[2].color = Color.red;
                    break;

                case 1:
                    dash[0].color = Color.green;
                    dash[1].color = Color.red;
                    dash[2].color = Color.red;
                    break;

                case 2:
                    dash[0].color = Color.green;
                    dash[1].color = Color.green;
                    dash[2].color = Color.red;
                    break;

                case 3:
                    dash[0].color = Color.green;
                    dash[1].color = Color.green;
                    dash[2].color = Color.green;
                    break;
            }
        }

        private void SetSelectedWeaponWithName(string name)
        {
            foreach(Weapon weapon in weapons)
            {
                if (weapon.gameObject.name == name)
                {
                    selectedWeapon = weapon;
                    break;
                }
            }
        }
    }
}