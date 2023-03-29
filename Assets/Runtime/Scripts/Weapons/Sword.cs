using Final_Survivors.Audio;
using Final_Survivors.Enemies;
using Final_Survivors.Weapons;
using UnityEngine;

namespace Final_Survivors
{
    public class Sword : Weapon
    {
        [SerializeField] private float damage;
        [SerializeField] private Collider swordCollider;
        public bool hasHit { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && swordCollider.enabled)
            {
                other.GetComponent<Enemy>().TakeDamage(damage, 2);
                if (!hasHit)
                { 
                    SoundManager.PlaySound(ref soundBank.enemyHitSword, audioSource, audioSource.volume / 3);
                    hasHit = true;
                }
            }
        }

        public void NewDmg(float value)
        {
            damage = value;
        }
    }
}
