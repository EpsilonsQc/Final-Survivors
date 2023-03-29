using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Final_Survivors.Enemies;
using Final_Survivors.Observer;
using Final_Survivors.Core;
using Final_Survivors.Player;
using Final_Survivors.UI.WinMenu;
using Final_Survivors.PowerUps;
using Final_Survivors.Environment;

namespace Final_Survivors
{
    public class SpawnManager : Subject, IObserver
    {
        [Header("Spawning distance from player")]
        [SerializeField] private float maxSpawnDistance = 50f;

        [Header("Spawn Rates for enemies 0 - NOSE, 1 - RAZOR, 2 - SPIDER, 3 - ROBOT, 4 - DRAGON")]
        [SerializeField] private float[] enemiesMinSpawnRate;
        [SerializeField] private float[] enemiesMaxSpawnRate;
        [SerializeField] private int level = 1;
        public int Level { get { return level; } set { level = value; } }
        public int nbNormalDeadBeforeElite = 5;
        public int nbEliteDeadBeforeBoss = 3;
        public int nbBossDeadBeforeNextStage = 1;
        public int nbCurrentElite = 0;
        public int nbCurrentBoss = 0;
        [SerializeField] private bool spawn = false;

        [SerializeField] private GameObject[] enemyElitePrefabs = new GameObject[5];
        [SerializeField] private GameObject[] enemyBossPrefabs = new GameObject[5];
        private Transform specialsParent;
        [SerializeField] private SpawnerProximity[] enemySpawner;

        private GameObject player;
        private Subject _playerSubject;
        private PowerUpSpawner powerUpSpawner;
        private Camera cam;
        private ScoreManager scoreManager;
        [SerializeField] private RadioCollision radio;
        private SpecialCrate specialCrate;
        private bool isPlayerInSecretRoom;
        public bool isBossFight = false;
        private Transform enemyParent;

        [SerializeField] public int nbMaxEnemiesFromPool;
        [SerializeField] public int enemiesPoolCount = 0;

        [Header("UI reference")]
        [SerializeField] private GameObject menuManager;

        private void Awake()
        {
            enemyParent = GameObject.FindGameObjectWithTag("Enemy").transform;
        }

        private void Start()
        {
            cam = Camera.main;
            player = GameObject.FindGameObjectWithTag("Player");
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            specialsParent = GameObject.FindGameObjectWithTag("Specials").transform;
            powerUpSpawner = GetComponent<PowerUpSpawner>();
            radio.AddObserver(this);
            specialCrate = GameObject.FindGameObjectWithTag("SpecialCrate").GetComponent<SpecialCrate>();
            specialCrate.AddObserver(this);
            specialCrate.gameObject.SetActive(false);

            StartCoroutine(nameof(SpawnEnemies));
        }

        private void OnEnable()
        {
            _playerSubject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerSubject.AddObserver(this);                  
        }

        public void OnNotify(Events action)
        {
            if (action == Events.SECRET_ROOM_IN)
            {
                isPlayerInSecretRoom = true;
                isBossFight = true;
                PauseSpawn();
                powerUpSpawner.PauseSpawnBallisticCrate();
                powerUpSpawner.PauseSpawnShieldCrate();
            }

            if (action == Events.SECRET_ROOM_OUT)
            {
                isPlayerInSecretRoom = false;
                isBossFight = false;
                PlaySpawn();
                powerUpSpawner.PlaySpawnBallisticCrate();
                powerUpSpawner.PlaySpawnShieldCrate();
            }

            if (!isPlayerInSecretRoom)
            { 
                if (action == Events.TIME_WARP_DAY_TO_NIGHT)
                {
                    PauseSpawn();
                    powerUpSpawner.PauseSpawnBallisticCrate();
                    powerUpSpawner.PauseSpawnShieldCrate();
                }

                if (action == Events.TIME_WARP_NIGHT_TO_DAY)
                {
                    PlaySpawn();
                    powerUpSpawner.PlaySpawnBallisticCrate();
                    powerUpSpawner.PlaySpawnShieldCrate();
                }
            }
        }

        private void SetParent(GameObject child, Transform parent)
        {
            child.transform.SetParent(parent);
        }

        private void Update()
        {
            if (!isPlayerInSecretRoom && !EnvironmentState.GetIsPause())
            {
                SpeedUpSpawn(); // Make the enemies appear more quickly over time

                if (!spawn && enemiesPoolCount < nbMaxEnemiesFromPool)
                    StartCoroutine(nameof(SpawnEnemies));
            }
        }

        public bool IsPlayerInSecretRoom()
        {
            return isPlayerInSecretRoom;
        }

        private void SpeedUpSpawn()
        {
            for (int i = 0; i < enemiesMinSpawnRate.Length; ++i)
            {
                if (enemiesMinSpawnRate[i] > 0.5)
                {
                    enemiesMinSpawnRate[i] -= Time.deltaTime / 1000;
                }
            }
        }

        public void CheckSpawnBiggerEnemies()
        {
            StartCoroutine(nameof(CheckTriggersBiggerEnemies));
        }

        public void CheckTriggersBiggerEnemies()
        {
            if (scoreManager.normalCounter >= nbNormalDeadBeforeElite)
            {
                Debug.Log("An ELITE has spawn !");
                NotifyObservers(Events.SPAWNELITE);
                SpawnElite();
                scoreManager.normalCounter = 0;
            }

            if (scoreManager.eliteCounter >= nbEliteDeadBeforeBoss && nbCurrentBoss == 0)
            {
                Debug.Log("A BOSS has spawn !");
                NotifyObservers(Events.SPAWNBOSS);
                /*int rand = UnityEngine.Random.Range(0, 2);
                int dragonOrSpider = rand == 0 ? 0 : 4;*/
                SpawnBoss(0);
                scoreManager.eliteCounter = 0;
            }

            if (scoreManager.bossCounter >= nbBossDeadBeforeNextStage)
            {
                Debug.Log("YOU WON !");
                PauseSpawn();
                powerUpSpawner.PauseSpawnBallisticCrate();
                powerUpSpawner.PauseSpawnShieldCrate();
                Invoke(nameof(EndGame), 5);
            }
        }

        private void EndGame()
        {
            Debug.Log("EndGame");
            NotifyObservers(Events.END_GAME);
            menuManager.GetComponent<WinMenu>().GameOver();
        }

        public void PauseSpawn()
        {
            StopAllCoroutines();

            // Tweek first instance of ennemy not in pool
            //enemyParent.GetChild(1).gameObject.GetComponent<Enemy>().SetSleepMode(true);
        }

        public void PlaySpawn()
        {
            StartCoroutine(nameof(SpawnEnemies));
        }

        public void SpawnElite(int index = -1)
        {
            //PauseSpawn();
            Enemy enemy;

            if (index != -1)
            {
                GameObject elite = Instantiate(enemyElitePrefabs[index]);
                SetParent(elite, specialsParent);
                elite.SetActive(true);
                enemy = elite.GetComponent<Enemy>();
                enemy.MaxHealth = (enemy.MaxHealth * Level) / 1.25f;
                enemy.Health = (enemy.Health * Level) / 1.25f;
            }
            else
            {
                enemy = GetRandomEliteEnemy();
            }

            CalculateSpawnPosition(enemy);
            ++nbCurrentElite;
        }

        public void SpawnBoss(int index = -1)
        {
            //PauseSpawn();
            isBossFight = true;
            Enemy enemy;

            if (index != -1)
            {
                GameObject boss = Instantiate(enemyBossPrefabs[index]);
                SetParent(boss, specialsParent);
                boss.SetActive(true);
                enemy = boss.GetComponent<Enemy>();
                enemy.MaxHealth = (enemy.MaxHealth * Level) / 1.25f;
                enemy.Health = (enemy.Health * Level) / 1.25f;
            }
            else
            {
                enemy = GetRandomBossEnemy();
            }

            CalculateSpawnPosition(enemy);
            ++nbCurrentBoss;
        }

        private Enemy GetRandomEliteEnemy()
        {
            int rand = UnityEngine.Random.Range(0, 5);
            GameObject elite = Instantiate(enemyElitePrefabs[rand]);
            SetParent(elite, specialsParent);
            elite.SetActive(true);
            Enemy enemyElite = elite.GetComponent<Enemy>();
            enemyElite.MaxHealth = (enemyElite.MaxHealth * Level) / 1.25f;
            enemyElite.Health = (enemyElite.Health * Level) / 1.25f;
            return enemyElite;
        }

        private Enemy GetRandomBossEnemy()
        {
            int rand = UnityEngine.Random.Range(0, 5);
            GameObject boss = Instantiate(enemyBossPrefabs[rand]);
            SetParent(boss, specialsParent);
            boss.SetActive(true);
            Enemy enemyBoss = boss.GetComponent<Enemy>();
            enemyBoss.MaxHealth = (enemyBoss.MaxHealth * Level) / 1.25f;
            enemyBoss.Health = (enemyBoss.Health * Level) / 1.25f;
            return enemyBoss;
        }

        public void SleepGreaterEnemies()
        {
            foreach (Transform child in specialsParent)
            {
                Enemy enemy = child.gameObject.GetComponent<Enemy>();
                if (enemy.isActiveAndEnabled)
                {
                    if (enemy.Level != EnemyLevel.BOSS)
                        enemy.SetSleepMode(true);
                    enemy.isAttacking = false;
                }
            }
        }

        public void WakeUpGreatersEnemies()
        {
            foreach (Transform child in specialsParent)
            {
                Enemy enemy = child.gameObject.GetComponent<Enemy>();
                if (enemy.isActiveAndEnabled && enemy.Level != EnemyLevel.BOSS)
                {
                    enemy.SetSleepMode(false);
                    enemy.Agent.enabled = false;
                    enemy.Agent.enabled = true;
                }
            }
        }

        private IEnumerator SpawnEnemies()
        {
            spawn = true;

            while (enemiesPoolCount + ObjectPooling.instance.GetNumberOfPools() < nbMaxEnemiesFromPool)
            {
                for (int y = 0; y < ObjectPooling.instance.GetNumberOfPools(); ++y)
                {
                    Enemy enemy = ObjectPooling.instance.TakeEnemyFromPool(y);

                    if (enemy != null)
                    {
                        CalculateSpawnPosition(enemy);
                        ++enemiesPoolCount;
                        yield return new WaitForSeconds(UnityEngine.Random.Range(enemiesMinSpawnRate[y] / level * 2, enemiesMaxSpawnRate[y] / level * 2));
                    }
                    else
                    {
                        //Debug.Log("Null");
                        continue;
                    }
                }
                break;
            }

            spawn = false;
        }

        private void CalculateSpawnPosition(Enemy obj)
        {
            int random = UnityEngine.Random.Range(0, enemySpawner.Length);

            while (enemySpawner[random].nearPlayer)
            {
                random = UnityEngine.Random.Range(0, enemySpawner.Length);
            }

            Vector3 randomPosition = enemySpawner[random].transform.position;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, maxSpawnDistance, NavMesh.AllAreas))
            {
                obj.transform.position = new Vector3(hit.position.x, hit.position.y + 5, hit.position.z);
            }
            /*else
            {
                Debug.Log("Failed to position object");
            }*/
        }
    }
}
