using System.Collections.Generic;
using UnityEngine;
using Final_Survivors.Projectile;
using Final_Survivors.Enemies;
using Final_Survivors.Observer;
using Final_Survivors.Core;
using Final_Survivors.Player;
using Final_Survivors.Weapons;

namespace Final_Survivors
{
    public class ObjectPooling : MonoBehaviour, IObserver
    {
        private Queue<Enemy>[] enemyPools = { new Queue<Enemy>(), new Queue<Enemy>(), new Queue<Enemy>(), new Queue<Enemy>(), new Queue<Enemy>() };
        private Queue<BulletEnemy> enemyBulletPool = new Queue<BulletEnemy>();

        [Header("Max Pool Sizes - Enemy")]
        [SerializeField] private int[] maxEnemySize = new int[5];

        [Header("Max Pool Sizes - Projectiles")]
        [SerializeField] private int enemyAmmoPoolSize;
        [SerializeField] private int projectilePoolSize;

        [Header("Prefabs")]
        [SerializeField] private GameObject[] enemyPrefabs = new GameObject[5];
        [SerializeField] private GameObject masterProjectilePrefab;
        [SerializeField] private GameObject enemyBulletPrefab;
        [SerializeField] private float enemyLatencyAfterWakeUp = 1f;

        public static ObjectPooling instance;
        private Queue<MasterProjectile> projectilePool = new Queue<MasterProjectile>();
        private Transform enemyParent;
        private Transform projectileParent;
        private Subject _playerSubject;
        private SpawnManager spawnManager;

        private void Awake()
        {
            instance = this;
            enemyParent = GameObject.FindGameObjectWithTag("Enemy").transform;
            projectileParent = GameObject.FindGameObjectWithTag("Projectiles").transform;
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        }

        private void Start()
        {
            PopulateEnemyPools();
            PopulateProjectilesPool();
        }

        private void OnEnable()
        {
            _playerSubject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerSubject.AddObserver(this);
        }

        private void OnDisable()
        {
            _playerSubject.RemoveObserver(this);
        }

        public void OnNotify(Events action)
        {
            if (!spawnManager.IsPlayerInSecretRoom())
            {
                if (action == Events.TIME_WARP_DAY_TO_NIGHT)
                {
                    SleepAllEnemies();
                    ReturnAllActiveProjectilesToPool();
                    ReturnAllActiveBulletsToPool();
                }

                if (action == Events.TIME_WARP_NIGHT_TO_DAY)
                {
                    Invoke("WakeUpAllEnemies", enemyLatencyAfterWakeUp);
                }
            }
        }

        public int GetNumberOfPools()
        {
            return enemyPools.Length;
        }

        private void PopulateProjectilesPool()
        {
            for (int i = 0; i < projectilePoolSize; i++)
            {
                GameObject bullet = Instantiate(masterProjectilePrefab);
                MasterProjectile bulletScript = bullet.GetComponent<MasterProjectile>();
                SetParent(bullet, projectileParent);
                projectilePool.Enqueue(bulletScript);
                bulletScript.SetupMaster();
                bullet.SetActive(false);
            }

            for (int i = 0; i < enemyAmmoPoolSize; i++)
            {
                GameObject bullet = Instantiate(enemyBulletPrefab);
                BulletEnemy bulletScript = bullet.GetComponent<BulletEnemy>();
                SetParent(bullet, projectileParent);
                enemyBulletPool.Enqueue(bulletScript);
                bullet.SetActive(false);
            }
        }

        private void PopulateEnemyPools()
        {
            for (int i = 0; i < enemyPrefabs.Length; ++i)
            {
                for (int y = 0; y < maxEnemySize[i]; ++y)
                {
                    GameObject enemy = Instantiate(enemyPrefabs[i]);
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    SetParent(enemy, enemyParent);
                    enemyPools[i].Enqueue(enemyScript);
                    enemy.SetActive(false);
                }
            }
        }

        public MasterProjectile TakeProjectilesFromPool(WeaponType weapon)
        {
            if (projectilePool.Count > 0)
            {
                MasterProjectile projectile = projectilePool.Dequeue();
                projectile.gameObject.SetActive(true);
                projectile.ActivateProjectile(weapon);
                projectilePool.Enqueue(projectile);
                return projectile;
            }
            else
            {
                GameObject bullet = Instantiate(masterProjectilePrefab);
                MasterProjectile projectile = bullet.GetComponent<MasterProjectile>();
                SetParent(bullet, projectileParent);
                projectile.ActivateProjectile(weapon);
                projectilePool.Enqueue(projectile);
                projectile.SetupMaster();
                return projectile;
            }
        }

        public Bullet TakeEnemyBulletFromPool()
        {
            if (enemyBulletPool.Count > 0)
            {
                BulletEnemy projectile = enemyBulletPool.Dequeue();
                projectile.gameObject.SetActive(true);
                enemyBulletPool.Enqueue(projectile);
                return projectile;
            }
            else
            {
                GameObject bullet = Instantiate(enemyBulletPrefab);
                BulletEnemy bulletScript = bullet.GetComponent<BulletEnemy>();
                SetParent(bullet, projectileParent);
                enemyBulletPool.Enqueue(bulletScript);
                return bulletScript;
            }
        }

        public Enemy TakeEnemyFromPool(int pool)
        {
            if (enemyPools[pool].Count > 0)
            {
                Enemy enemy = enemyPools[pool].Dequeue();
                enemyPools[pool].Enqueue(enemy);

                if (!enemy.gameObject.activeSelf)
                {
                    enemy.gameObject.SetActive(true);
                    return enemy;
                }
            }

            return null;
        }

        public void ReturnObjToPool(Enemy obj, EnemyName name)
        {
            switch (name)
            {
                case EnemyName.NOSE:
                    enemyPools[0].Enqueue(obj);
                    obj.gameObject.SetActive(false);
                    break;
                case EnemyName.RAZOR:
                    enemyPools[1].Enqueue(obj);
                    obj.gameObject.SetActive(false);
                    break;
                case EnemyName.SPIDER:
                    enemyPools[2].Enqueue(obj);
                    obj.gameObject.SetActive(false);
                    break;
                case EnemyName.ROBOT:
                    enemyPools[3].Enqueue(obj);
                    obj.gameObject.SetActive(false);
                    break;
                case EnemyName.DRAGON:
                    enemyPools[4].Enqueue(obj);
                    obj.gameObject.SetActive(false);
                    break;
            }

            if (spawnManager.enemiesPoolCount > 0)
                --spawnManager.enemiesPoolCount;
        }

        public void ReturnProjectilesToPool(MasterProjectile script)
        {
            projectilePool.Enqueue(script);
            script.gameObject.SetActive(false);
        }

        public void ReturnEnemyBulletToPool(BulletEnemy script)
        {
            enemyBulletPool.Enqueue(script);
            script.gameObject.SetActive(false);
        }

        private void SetParent(GameObject child, Transform parent)
        {
            child.transform.SetParent(parent);
        }

        private void ReturnAllActiveProjectilesToPool()
        {
            foreach (MasterProjectile bullet in projectilePool)
            {
                if (bullet.isActiveAndEnabled)
                    bullet.gameObject.SetActive(false);
            }
        }

        private void ReturnAllActiveBulletsToPool()
        {
            foreach (BulletEnemy bullet in enemyBulletPool)
            {
                if (bullet.isActiveAndEnabled)
                    bullet.gameObject.SetActive(false);
            }
        }

        public void SleepAllEnemies()
        {
            foreach (Queue<Enemy> queueEnemy in enemyPools)
            {
                foreach (Enemy enemy in queueEnemy)
                {
                    if (enemy.isActiveAndEnabled)
                        enemy.SetSleepMode(true);
                }
            }

            spawnManager.SleepGreaterEnemies();
        }

        public void WakeUpAllEnemies()
        {
            foreach (Queue<Enemy> queueEnemy in enemyPools)
            {
                foreach (Enemy enemy in queueEnemy)
                {
                    enemy.SetSleepMode(false);
                    enemy.Agent.enabled = false;
                    enemy.Agent.enabled = true;
                }
            }

            spawnManager.WakeUpGreatersEnemies();
        }
    }
}
