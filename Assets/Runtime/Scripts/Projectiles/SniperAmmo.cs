using Final_Survivors.Enemies;
using Final_Survivors.Audio;
using UnityEngine;
using System.Collections;

namespace Final_Survivors.Projectile
{
    public class SniperAmmo : Bullet
    {
        [SerializeField] private MasterProjectile master;

        private void Update()
        {
            if (lifeTime <= 0)
            {
                NotifyObservers(Core.Events.RETURN_BULLET);
            }
            lifeTimer -= Time.deltaTime;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (enabled)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<Enemy>().TakeDamage(damageAmount);
                    SoundManager.PlaySound(ref master.soundBank.enemyHitBallistic, master.source, master.source.volume / 3);
                }
                if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("FL_Collider") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("EnemySpawner") && !collision.CompareTag("Radio"))
                {                 
                    NotifyObservers(Core.Events.RETURN_BULLET);
                }
            }
        }
    }
}
