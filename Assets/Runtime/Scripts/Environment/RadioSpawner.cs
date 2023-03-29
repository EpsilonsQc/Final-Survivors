using UnityEngine;
using Final_Survivors.Observer;
using Final_Survivors.Core;
using Final_Survivors.Player;

namespace Final_Survivors
{
    public class RadioSpawner : MonoBehaviour, IObserver
    {
        //[SerializeField] private float maxSpawningDistance = 70;
        //[SerializeField] private float minSpawningDistance = 20;
        [SerializeField] private GameObject radioPrefab;
        private GameObject player;
        private GameObject radio;
        private Subject _playerSubject;

        void Awake()
        {
            _playerSubject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            radio = GameObject.FindGameObjectWithTag("Radio");

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        private void Start()
        {
            //SpawnRadio();
            //radio.SetActive(false);
        }
        private void OnEnable()
        {
            _playerSubject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerSubject.AddObserver(this);
        }

        private void OnDisable()
        {
            _playerSubject.RemoveObserver(this);
        }

        public void OnNotify(Events action)
        {
            if (action == Events.SECRET_ROOM_IN)
            {
                if (radio.activeSelf)
                    radio.SetActive(false);
            }

            if (action == Events.SECRET_ROOM_OUT)
            {
                if (! radio.activeSelf)
                    radio.SetActive(true);
            }
        }
        //private void SpawnRadio()
        //{
        //    radio = Instantiate(radioPrefab);
        //    //SetParent(shieldCrate, cratesParent);
        //    radio.transform.position = new Vector3(21, -5, 57);
        //}
        //private Vector3 RandomSpawn(GameObject obj)
        //{
        //    float randomNumberX = Random.Range(minSpawningDistance, maxSpawningDistance);
        //    float randomNumberZ = Random.Range(minSpawningDistance, maxSpawningDistance);

        //    Vector3 randomPosition = player.transform.position + new Vector3(randomNumberX, obj.transform.position.y, randomNumberZ);

        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(randomPosition, out hit, Mathf.Infinity, NavMesh.AllAreas))
        //    {
        //        Vector3 spawnPosition = new Vector3(hit.position.x, hit.position.y + 0.2f, hit.position.z);
        //        transform.position = spawnPosition;
        //    }

        //    return transform.position;
        //}

    }
}
