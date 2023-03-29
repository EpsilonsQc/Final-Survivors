using Final_Survivors.Enemies;
using UnityEngine;
using System.Collections;
using Final_Survivors.Audio;

namespace Final_Survivors.Projectile
{
    public class Rocket : Bullet
    {
        [SerializeField] private float explosionRadius;
        [SerializeField] private ParticleSystem fx;
        [SerializeField] private MasterProjectile master;

        private void Update()
        {
            if (lifeTime <= 0)
            {
                NotifyObservers(Core.Events.RETURN_BULLET);
            }
            lifeTimer -= Time.deltaTime;
        }

        private void OnEnable()
        {
            rb.velocity= Vector3.zero;
            transform.forward = direction;
            transform.localScale = Vector3.one;
        }

        public override void AddForce(Vector3 direction, Vector3 position)
        {
            transform.position = position;
            transform.forward = -direction;
            rb.AddForce(projectileSpeed * direction, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (enabled)
            {
                if (!collision.gameObject.CompareTag("FL_Collider") && !collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("EnemySpawner") && !collision.CompareTag("Radio"))
                {
                    fx.Play();

                    Collider[] hitColliders = Physics.OverlapSphere(collision.transform.position, explosionRadius, (1 << 6));

                    foreach(Collider col in hitColliders)
                    {
                        col.GetComponent<Enemy>().TakeDamage(damageAmount, 1);
                    }


                    rb.velocity = Vector3.zero;
                    rb.Sleep();
                    rb.detectCollisions = false;
                    SoundManager.PlaySound(ref master.soundBank.enemyHitExplosion, master.source, master.source.volume / 3);

                    StartCoroutine(nameof(WaitForDestroy));
                }                  
            }
        }

        IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(.5f);
            rb.WakeUp();
            rb.detectCollisions|= true;
            NotifyObservers(Core.Events.RETURN_BULLET);
        }
    }
}
