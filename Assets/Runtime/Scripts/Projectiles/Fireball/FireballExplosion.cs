using Final_Survivors.Player;
using Final_Survivors.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Final_Survivors
{
    public class FireballExplosion : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private float timer = 0;
        [SerializeField] private Fireball fireball;
        [SerializeField] private Collider ExplosionCollider;
        private bool hasCollided;

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                Destroy(transform.parent.parent.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hasCollided)
            {
                if (other.CompareTag("Player"))
                {
                    //Debug.Log("Boom!!!");
                    other.GetComponent<PlayerManager>().TakeDamage(fireball.damage);
                    transform.localPosition = new Vector3(0, 0, 0);
                    hasCollided = true;
                }
            }
        }
    }
}
