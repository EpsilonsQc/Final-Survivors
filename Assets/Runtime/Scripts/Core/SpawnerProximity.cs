using UnityEngine;

namespace Final_Survivors.Core
{
    public class SpawnerProximity : MonoBehaviour
    {
        [SerializeField] public bool nearPlayer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                nearPlayer = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
                nearPlayer = true;        
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                nearPlayer = false;
        }
    }
}
