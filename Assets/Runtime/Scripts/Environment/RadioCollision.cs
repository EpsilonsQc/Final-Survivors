using Final_Survivors.Audio;
using Final_Survivors.Observer;
using System.Collections;
using UnityEngine;

namespace Final_Survivors
{
    public class RadioCollision : Subject
    {
        private bool isActive = false;
        private bool isReadyToFade = false;
        private bool removeFadeReady = false;
        private CanvasGroup radioCanvas;
        private GameObject player;
        [SerializeField] private AudioClip audioclip;
        [SerializeField] private Vector3 playerTeleport;
        private AudioSource audioSource;
        private float counter;

        private void Awake()
        {
            radioCanvas = GameObject.Find("RadioCanvas").GetComponent<CanvasGroup>();
            player = GameObject.FindGameObjectWithTag("Player");
            radioCanvas.alpha = 0;
            audioSource = gameObject.GetComponent<AudioSource>();
            counter = 0;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (gameObject.activeSelf)
                {
                    counter += Time.deltaTime;

                    if (counter >= 3)
                    {
                        isActive = true;
                    }

                    if (isActive)
                    {
                        AudioClip[] ac = new AudioClip[1];
                        ac.SetValue(audioclip, 0);
                        SoundManager.PlaySound(ref ac, audioSource, audioSource.volume / 3);
                        isReadyToFade = true;
                        isActive = false;
                        StartCoroutine(nameof(TeleportFadeIn));
                        NotifyObservers(Core.Events.SECRET_ROOM_IN);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                counter = 0;
            }
        }

        IEnumerator TeleportFadeOut()
        {
            player.transform.position = playerTeleport;
            isReadyToFade = false;
            removeFadeReady = true;

            if (removeFadeReady)
            {
                while (radioCanvas.alpha > 0f)
                {
                    radioCanvas.alpha -= 0.50f * Time.deltaTime;
                    yield return null;
                }
            }

            isReadyToFade = true;
            removeFadeReady = false;
            ObjectPooling.instance.SleepAllEnemies();
            gameObject.SetActive(false);
        }

        IEnumerator TeleportFadeIn()
        {
            if (isReadyToFade)
            {
                while (radioCanvas.alpha < 0.98f)
                {
                    radioCanvas.alpha += 0.50f * Time.deltaTime;
                    yield return null;
                }
            }
            
            StartCoroutine(nameof(TeleportFadeOut));
            StopCoroutine(nameof(TeleportFadeIn));
        }
    }
}
