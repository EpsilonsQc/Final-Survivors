using Final_Survivors.Observer;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Final_Survivors.PowerUps
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [Header("Spawning distance from player")]
        [SerializeField] private float maxSpawningDistance = 20;

        [Header("Shield Crate")]
        [SerializeField] private GameObject shieldCratePrefab;
        [SerializeField] private float shieldSpawnRate = 12;

        [Header("Ballistic Crate")]
        [SerializeField] private GameObject ballisticCratePrefab;
        [SerializeField] private float ballisticSpawnRate = 12;

        // References
        private GameObject player;
        private Subject _playerSubject;
        private Transform cratesParent;

        private void Awake()
        {
            cratesParent = GameObject.FindGameObjectWithTag("Crates").transform;

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }

            StartCoroutine("SpawnShieldCrate");
            StartCoroutine(nameof(SpawnBallisticCrate));
        }

        private IEnumerator SpawnBallisticCrate()
        {
            while (true) // to change to while (game not over)
            {
                GameObject ballisticCrate = Instantiate(ballisticCratePrefab);
                SetParent(ballisticCrate, cratesParent);
                ballisticCrate.transform.position = RandomSpawn(ballisticCrate);
                yield return new WaitForSeconds(ballisticSpawnRate);
            }
        }

        public void PlaySpawnBallisticCrate()
        {
            StartCoroutine(nameof(SpawnBallisticCrate));
        }

        public void PauseSpawnBallisticCrate()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnShieldCrate()
        {
            while (true) // to change to while (game not over)
            {
                GameObject shieldCrate = Instantiate(shieldCratePrefab);
                SetParent(shieldCrate, cratesParent);
                shieldCrate.transform.position = RandomSpawn(shieldCrate);
                yield return new WaitForSeconds(shieldSpawnRate);
            }
        }

        public void PlaySpawnShieldCrate()
        {
            StartCoroutine(nameof(SpawnShieldCrate));
        }

        public void PauseSpawnShieldCrate()
        {
            StopCoroutine(nameof(SpawnShieldCrate));
        }

        private Vector3 RandomSpawn(GameObject obj)
        {
            float randomNumberX = Random.Range(-maxSpawningDistance, maxSpawningDistance);
            float randomNumberZ = Random.Range(-maxSpawningDistance, maxSpawningDistance);

            Vector3 randomPosition = player.transform.position + new Vector3(randomNumberX, obj.transform.position.y, randomNumberZ);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                Vector3 spawnPosition = new Vector3(hit.position.x, hit.position.y + 0.2f, hit.position.z);
                transform.position = spawnPosition;
            }

            return transform.position;
        }

        private void SetParent(GameObject child, Transform parent)
        {
            child.transform.SetParent(parent);
        }
    }
}
