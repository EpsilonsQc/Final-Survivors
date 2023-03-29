using Final_Survivors.Core;
using Final_Survivors.Player;
using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class Guards : Enemy
    {
        [SerializeField] private float meleeDamage;

        private void Awake()
        {
            Level = EnemyLevel.QUEEN_GUARD;
            Type = EnemyType.MELEE;
            animator = gameObject.GetComponent<Animator>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            Agent.enabled = false;
            Agent.enabled = true;
        }

        private void Update()
        {
            LookAtPlayer();
        }

        protected override void LookAtPlayer()
        {
            if (!IsDead)
            {
                transform.LookAt(playerTransform.position);
            }
        }

        public void DealDamage()
        {
            if (Vector3.Distance(playerTransform.position, transform.position) <= attackRange)
            {
                playerTransform.GetComponent<PlayerManager>().TakeDamage(meleeDamage);
            }

            AttackCooldown();
        }
    }
}
