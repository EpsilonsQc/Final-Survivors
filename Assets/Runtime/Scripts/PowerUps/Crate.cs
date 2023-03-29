using Final_Survivors.Environment;
using UnityEngine;
using System.Collections;

namespace Final_Survivors.PowerUps
{
    public abstract class Crate : MonoBehaviour
    {
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private bool DisplayByNight = false;
        //[SerializeField] private GameObject pickupFX;
        [SerializeField] private ParticleSystem pickupFX;
        [SerializeField] private GameObject aura;
        [SerializeField] private GameObject icon;
        private bool isPickedUp = false;

        public abstract void SetPowerUp();

        private void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
            mesh.enabled = true;
            //pickupFX.SetActive(false);
            isPickedUp = false;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SetPowerUp();
                mesh.enabled = false;
                aura.SetActive(false);
                icon.SetActive(false);
                StartCoroutine(nameof(GetPickedUp));
                if(isPickedUp)
                {
                    DestroyCrate();
                    //pickupFX.SetActive(false);
                }
                
            }

            if (other.gameObject.CompareTag("FL_Collider") && !EnvironmentState.GetIsDay())
            {
                Display(true);
            }
        }

        IEnumerator GetPickedUp()
        {
            yield return new WaitForSeconds(0.02f);
            pickupFX.Play();
            //pickupFX.SetActive(true);
            isPickedUp = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("FL_Collider") && !EnvironmentState.GetIsDay())
            {
                Display(false);
            }
        }

        public void DestroyCrate()
        {
            // Add effect
            Destroy(gameObject);
        }

        private void Display(bool value)
        {
            if (DisplayByNight)
            {
                mesh.enabled = value;
            }
        }
    }
}
