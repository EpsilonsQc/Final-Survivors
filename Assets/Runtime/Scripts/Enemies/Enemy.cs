using UnityEngine;
using Final_Survivors.Core;
using Final_Survivors.Observer;
using Final_Survivors.Environment;
using System.Collections;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Final_Survivors.Enemies
{
    public enum EnemyName {DRAGON, NOSE, RAZOR, ROBOT, SPIDER}
    public enum EnemyLevel {NORMAL, ELITE, BOSS, QUEEN_GUARD}
    public enum EnemyType {RANGED, MELEE}

    public class Enemy : Subject, IDamageModifier
    {
        [Header("Health")]
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;

        [Header("Attack")]
        [SerializeField] public float attackSpeed;
        [SerializeField] public float attackRange;

        [Header("Movement")]
        [SerializeField] NavMeshAgent agent;
        [SerializeField] public float moveSpeed;

        [Header("Death")]
        [SerializeField] private float deathTime;
        [SerializeField] private bool isDead;
        private float deathTimer;

        [Header("Collider")]
        [SerializeField] private Collider _collider;

        [Header("Sleep HUD")]
        [SerializeField] public ParticleSystem sleep;
        [SerializeField] private bool isSleepMode;
        [SerializeField] private ParticleSystem alertFx;
        [SerializeField] private ParticleSystem wakeUpFX;

        public Animator animator { get; set; }       
        private EnemyType type;
        private EnemyLevel level;
        [SerializeField] private EnemyName enemyName;
        private float attackTimer;
        private int awakeTimer = 0;
        private const int awake = 100;
        private const float spreadRadius = 3;
        public Transform playerTransform { get; set; }
        public bool isAttacking { get; set; }
        private const float slowDamageCD = 0.25f;
        private float speed = 0f;
        private int takenDamage = 0;
        private Events damageModifierEvt = Events.NORMAL_DMG;
        private SpawnManager spawnManager;
        private ScoreManager scoreManager;
        [SerializeField] private GameObject explosion;

        // Properties (Getters and Setters)
        public float Health { get { return health; } set { health = value; } }
        public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public bool IsDead { get { return isDead; } }
        public EnemyType Type { get { return type; } protected set { type = value; } }
        public EnemyLevel Level { get { return level; } protected set { level = value; } }
        public EnemyName Name { get { return enemyName; } protected set { enemyName = value; } }
        public bool IsSleepMode { get { return isSleepMode; } private set { isSleepMode = value; } }
        public float AttackTimer { get { return attackTimer; } protected set { attackTimer = value; } }
        public int AwakeTimer { get { return awakeTimer; } protected set { awakeTimer = value; } }
        public NavMeshAgent Agent { get { return agent; } }
        public int TakenDamage { get { return takenDamage; } private set { takenDamage = value; } }
        public Events DamageModifierEvt { get { return damageModifierEvt; } private set { damageModifierEvt = value; } }

        private void Start()
        {
            speed = moveSpeed;
            health = maxHealth;
            isSleepMode = false;
            isAttacking = false;
            animator = gameObject.GetComponent<Animator>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

            explosion.SetActive(false);
        }

        private void Update()
        {
            if (level != EnemyLevel.QUEEN_GUARD)
            {
                if (!isSleepMode && !EnvironmentState.GetIsDay() && !isDead)
                {
                    AlertOthers();
                    SetSleepFx(false);
                }
                
                /*
                else if (EnvironmentState.GetIsDay() && !spawnManager.IsPlayerInSecretRoom())
                    SetSleepMode(false);
                */
                if (!spawnManager.IsPlayerInSecretRoom() && !IsDead && !IsSleepMode)
                    LookAtPlayer();
            }
        }

        private void OnEnable()
        {
            health = maxHealth;
            isDead = false;
            _collider.enabled = true;
            attackTimer = attackSpeed;
            deathTimer = deathTime;
            agent.enabled = false;
            agent.enabled = true;
        }

        protected virtual void LookAtPlayer()
        {
            if (!isDead && !isAttacking && !isSleepMode)
                transform.LookAt(playerTransform.position);
        }

        public void SetSleepMode(bool value)
        {
            if (value)
                StopAttack();
            else
                SetSleepFx(value);

            isSleepMode = value;
        }

        public void SetAwakeTimer(int value)
        {
            awakeTimer = value;
        }

        IEnumerator WaitExplosion()
        {
            yield return new WaitForSeconds(0.5f);
            explosion.SetActive(false);
        }

        public void TakeDamage(float damage, int soundType = 0)
        {
            TakenDamage = DamageModifier((int) damage);
            explosion.SetActive(true);
            StartCoroutine(nameof(WaitExplosion));

            if (health > TakenDamage)
            {
                //explosion.SetActive(true);
                //if (Level != EnemyLevel.BOSS)
                //    animator.Play("Take Damage", 0);

                health -= TakenDamage;
               
                //if(level != EnemyLevel.BOSS)
                //    moveSpeed = 0;

                //StartCoroutine(nameof(DamageSlowCooldown));
            }
            else
            {
                SetAlertFx(false);

                health = 0;
                isDead = true;
                _collider.enabled = false;

                if (this.name != "SecretRoomSpiderBoss" && level != EnemyLevel.QUEEN_GUARD)
                {
                    if (this.Level == EnemyLevel.ELITE)
                        --spawnManager.nbCurrentElite;
                    else if (this.Level == EnemyLevel.BOSS)
                        --spawnManager.nbCurrentBoss;

                    scoreManager.AddScore(Level);
                    spawnManager.CheckSpawnBiggerEnemies();
                }

                /*if (Level == EnemyLevel.ELITE) // Relaunch Spawn when Elite dies
                {
                    spawnManager.PlaySpawn();
                }*/
                
                InvokeRepeating(nameof(Dying), 0, Time.deltaTime);
            }

            SetSleepMode(false);
            NotifyObservers(Events.TAKE_DAMAGE);
            NotifyObservers(DamageModifierEvt);
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

        public IEnumerator DamageSlowCooldown()
        {
            yield return new WaitForSeconds(slowDamageCD);

            moveSpeed = speed;
        }

        public void AttackCooldown()
        {
            attackTimer = 0;
            InvokeRepeating(nameof(ResetAttackTimer), 0, Time.deltaTime);
        }

        private void ResetAttackTimer()
        {
            if (attackTimer >= attackSpeed)
                CancelInvoke(nameof(ResetAttackTimer));

            attackTimer += Time.deltaTime;
        }

        private void Dying()
        {
            if (deathTimer <= 0)
            {
                CancelInvoke(nameof(Dying));
                NotifyObservers(Events.RESET_ENEMY);
                
                if (Level == EnemyLevel.BOSS)
                    spawnManager.isBossFight = false;

                ObjectPooling.instance.ReturnObjToPool(this, enemyName);
            }

            deathTimer -= Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FL_Collider"))
            {
                NotifyObservers(Events.INSIDE_FLASHLIGHT);

                if (isSleepMode)
                {
                    SetAwakeTimer(0);
                    SetSleepFx(true);
                }
                else
                    SetSleepFx(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("FL_Collider"))
            {
                if (isSleepMode)
                {
                    SetSleepFx(true);
                    SetAwakeTimer(++awakeTimer);

                    if (awakeTimer >= awake)
                        SetSleepMode(false);
                }
                else
                    SetSleepFx(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("FL_Collider"))
            {
                SetAwakeTimer(0);
                SetSleepFx(false);
                NotifyObservers(Events.OUTSIDE_FLASHLIGHT);
            }
        }

        public void AlertOthers()
        {
            SetAlertFx(true);

            Collider[] cols = Physics.OverlapSphere(transform.position, spreadRadius, (1 << 6));

            foreach (var hit in cols)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.gameObject.GetComponent<Enemy>();

                    if (enemy.isSleepMode)
                        enemy.SetSleepMode(false);
                }
            }
        }

        private void SetSleepFx(bool value) 
        {
            if (value)
            {
                if (!sleep.isPlaying)
                    sleep.Play();
            }
            else
                sleep.Stop();
        }

        private void SetAlertFx(bool value)
        {
            if (value)
            {
                if (!alertFx.isPlaying)
                    alertFx.Play();
            }
            else
                alertFx.Stop();
        }

        public void ClearUI()
        {
            NotifyObservers(Events.TIME_WARP_DAY_TO_NIGHT);
        }

        public void StopAttack()
        {
            isAttacking = false;
        }
    }
}
