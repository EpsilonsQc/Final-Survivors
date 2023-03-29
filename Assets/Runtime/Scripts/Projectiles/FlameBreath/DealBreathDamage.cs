using Final_Survivors.Enemies;
using Final_Survivors.Player;
using UnityEngine;

namespace Final_Survivors
{
    public class DealBreathDamage : MonoBehaviour
    {
        [SerializeField] private DragonBoss boss;
        private ParticleSystem parts;

        private void Awake()
        {
            parts = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if(other.CompareTag("Player"))
            {
                other.GetComponent<PlayerManager>().TakeDamage(boss.BreathDamage * Time.deltaTime);
            }
        }
    }
}
