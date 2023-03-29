using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class QueenGuard : MeleeEnemy
    {
        private void Awake()
        {
            Level = EnemyLevel.QUEEN_GUARD;
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
    }
}
