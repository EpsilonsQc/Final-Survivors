using Final_Survivors.Audio;
using Final_Survivors.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Final_Survivors.Enemies
{
    public class SpiderBoss : Enemy
    {
        [SerializeField] private Transform mouth;
        [SerializeField] private float rotSpeed;

        [Header("WebBallSettings")]
        [SerializeField] private GameObject webBallPrefab;
        [SerializeField] private float webBallRange;
        [SerializeField] private float webBallCooldown;
        [SerializeField] private float webBallSpeed;
        [SerializeField] private float webBallSlowDuration;
        [SerializeField] private float slowStrength;
        private float webCooldownTimer = 0f;
        private bool isWebBallReady = true;

        [Header("Queen'sGuardSettings")]
        [SerializeField] private GameObject guardPrefab;
        [SerializeField] private int guardAmount;
        [SerializeField] private float spawnRadius;
        [SerializeField] private float spawnAtHPPercent;
        [SerializeField] private GameObject[] queenGuards;
        private bool isSpawnReady = true;

        [Header("MeleeSettings")]
        [SerializeField] private float meleeDamage;

        private SoundBank soundBank;
        private AudioSource source;

        public float WebBallRange { get { return webBallRange; } }
        public bool IsWebBallReady { get { return isWebBallReady; } }
        public bool IsSpawnReady { get { return isSpawnReady; } }
        public float SpawnAtHPPercent { get { return spawnAtHPPercent; } }

        private void Awake()
        {
            Level = EnemyLevel.BOSS;
            AttackTimer = attackSpeed;
            soundBank = FindObjectOfType<SoundBank>();
            source = GameObject.Find("FX").GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!IsDead)
                RotateTowardsPlayer();
        }

        private void RotateTowardsPlayer()
        {
            Vector3 newForward = (playerTransform.position - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, newForward, rotSpeed * Time.deltaTime);
        }

        public void MeleeAttack()
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                playerTransform.GetComponent<PlayerManager>().TakeDamage(meleeDamage);
            }
        }

        public void StopMelee()
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
            AttackCooldown();
        }

        public void ShootWebBall()
        {
            Vector3 dir = (playerTransform.position - mouth.position).normalized;
            GameObject go = Instantiate(webBallPrefab);
            go.GetComponent<WebBall>().SetupWebBall(mouth.transform.position, dir, webBallSpeed, webBallSlowDuration, slowStrength);
        }

        public void StopWebBall()
        {
            isAttacking= false;
            isWebBallReady = false;
            StartCoroutine(nameof(WebBallCooldown));
        }

        private IEnumerator WebBallCooldown()
        {
            while(webCooldownTimer < webBallCooldown)
            {
                webCooldownTimer+= Time.deltaTime;
                yield return null;
            }

            webCooldownTimer= 0f;
            isWebBallReady= true;
        }

        public void PlaySpawnScreamSound()
        {
            SoundManager.PlaySound(ref soundBank.spiderQueenScream, source, source.volume / 2);
        }
        public void SpawnQueenGuards()
        {      
            foreach(GameObject guards in queenGuards)
            {
                guards.SetActive(true);
            }

            /*
            for (int i = 0; i < guardAmount; ++i)
            {
                Vector2 spawnOffset =  Random.insideUnitCircle.normalized * spawnRadius;
                Vector3 newSpawn = transform.position + new Vector3(spawnOffset.x, 0, spawnOffset.y);
                if (SearchRandomSpwan(newSpawn, spawnRadius, out Vector3 hit))
                {
                    GameObject go = Instantiate(guardPrefab, this.transform.parent.transform);
                    go.transform.position = newSpawn;
                }
            }
            */
        }

        /*private bool SearchRandomSpwan(Vector3 position, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = position + Random.insideUnitSphere * range;
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }*/

        public void StopSpawn()
        {
            Debug.Log("Spawn complete");
            isSpawnReady = false;
            isAttacking = false;
        }
    }
}

