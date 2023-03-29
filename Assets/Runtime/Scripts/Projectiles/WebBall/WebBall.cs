using Final_Survivors.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Final_Survivors
{
    public class WebBall : MonoBehaviour
    {
        [SerializeField] private float trackingStrength;
        [SerializeField] private float lifeTime;
        private Vector3 direction;
        private float speed;
        private float slowDuration;
        private float slowStrength;
        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        private void Update()
        {
            lifeTime -= Time.deltaTime;

            if(lifeTime <= 0)
            {
                Destroy(gameObject);
            }

            Vector3 newDir = (playerTransform.position - transform.position).normalized;
            direction = Vector3.Lerp(direction, newDir, trackingStrength * Time.deltaTime);
            transform.position += newDir * speed * Time.deltaTime;
        }

        public void SetupWebBall(Vector3 position, Vector3 direction, float speed, float slowDuration, float slowStrength)
        {
            transform.position = position;
            this.direction = direction;
            this.speed = speed;
            this.slowDuration = slowDuration;
            this.slowStrength = slowStrength;
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerManager>().SetSlow(slowStrength, slowDuration);
                Destroy(gameObject);
            }
        }
    }
}
