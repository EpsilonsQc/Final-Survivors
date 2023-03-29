using Final_Survivors.Observer;
using UnityEngine;

namespace Final_Survivors.Projectile
{
    public class Projectile : Subject
    {
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected float damageAmount;
        [SerializeField] protected float lifeTime;
        [SerializeField] protected AmmoType type;
        protected float lifeTimer;
        protected Rigidbody rb;
        public Vector3 direction { get; set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            lifeTimer = lifeTime;
        }

        private void OnDisable()
        {
            lifeTimer = lifeTime;
        }
    }
}
