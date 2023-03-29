using UnityEngine;
using Final_Survivors.Player;

namespace Final_Survivors.Projectile
{
    public class BulletEnemy : Bullet
    {
        private void Update()
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime < 0)
            {
                ObjectPooling.instance.ReturnEnemyBulletToPool(this);
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("FL_Collider") && !collision.gameObject.CompareTag("EnemySpawner"))
            { 
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damageAmount);
                }
                ObjectPooling.instance.ReturnEnemyBulletToPool(this);
            }
        }
    }
}
