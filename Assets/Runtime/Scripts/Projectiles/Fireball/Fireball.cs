using Final_Survivors.Enemies;
using Final_Survivors.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Final_Survivors.Projectile
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] FireballCollision fireTrail;
        public float damage { get; private set; }
        private Vector3 direction;
        private float speed;
        public bool hasCollided { get; set; }

        public void SetupFireball(float damage, Vector3 position, Vector3 direction, float speed)
        {
            this.damage = damage;
            transform.position = position;
            this.direction = direction;
            this.speed = speed;

            fireTrail.damage = this.damage;
        }

        // Update is called once per frame
        void Update()
        {
            if (!hasCollided)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
}
