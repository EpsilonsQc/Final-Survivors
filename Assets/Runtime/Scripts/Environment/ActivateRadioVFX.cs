using UnityEngine;

namespace Final_Survivors
{
    public class ActivateRadioVFX : MonoBehaviour
    {
        private bool isActive = false;
        [SerializeField] private GameObject radio;
        private float radioCounter;

        private void Awake()
        {
            radioCounter = 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
               // Debug.Log("OnENTER: " + radioCounter);
                radioCounter = 0f;
                //Debug.Log("OnENTER: " + radioCounter);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                radioCounter += Time.deltaTime;
                //Debug.Log(Time.deltaTime);
                //Debug.Log("Ontriggerstay : " + radioCounter);
                if (radioCounter >= 5)
                {
                    isActive = true;
                    radioCounter = 0f;
                }
               
                if (isActive)
                {
                    radio.SetActive(true);
                    radioCounter = 0f;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                //Debug.Log("onexit" + radioCounter);
                radioCounter = 0f;
            }
        }

        //IEnumerator Wait5Seconds()
        //{
        //    yield return new WaitForSeconds(5);
        //    isActive = true;
        //}
    }
}
