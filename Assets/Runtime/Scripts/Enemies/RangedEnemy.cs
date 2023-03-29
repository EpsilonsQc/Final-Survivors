using Final_Survivors.Projectile;
using UnityEngine;

namespace Final_Survivors.Enemies
{
    public class RangedEnemy : Enemy
    {
        [SerializeField] Transform[] shoots;

        private void Awake()
        {
            Type = EnemyType.RANGED;
            Level = EnemyLevel.NORMAL;
        }

        public void ShootProjectile()
        {
            foreach (Transform shoot in shoots)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

                Bullet bullet = ObjectPooling.instance.TakeEnemyBulletFromPool();
                bullet.AddForce(directionToPlayer, shoot.position);
            }

            AttackCooldown();
        }
    }
}
