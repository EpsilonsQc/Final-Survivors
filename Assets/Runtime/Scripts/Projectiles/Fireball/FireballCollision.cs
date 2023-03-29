using Final_Survivors.Player;
using UnityEngine;

namespace Final_Survivors.Projectile
{
    public class FireballCollision : MonoBehaviour
    {
        [SerializeField] private Fireball fireball;
        [SerializeField] private GameObject explosionSystem;
        private ParticleSystem fireballParts;
        public float damage { get; set; }

        private void Awake()
        {
            fireballParts= GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            fireball.hasCollided = true;
            fireballParts.Stop();
            explosionSystem.SetActive(true);
        }
    }
}
