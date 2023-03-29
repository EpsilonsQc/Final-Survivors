using Final_Survivors.Audio;
using Final_Survivors.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Final_Survivors.Player;
using Final_Survivors.Core;

namespace Final_Survivors.PowerUps
{
    public class SpecialCrate : Subject
    {
        [SerializeField] private GameObject player;
        [SerializeField] private List<AudioClip> clipList = new List<AudioClip>();
        [SerializeField] private CanvasGroup radioCanvas;
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private ParticleSystem fx;
        [SerializeField] private GameObject aura;
        [SerializeField] private Vector3 playerTeleport;
        PlayerManager playerManager;
        private AudioSource source;
        private bool isReadyToFade = false;
        private bool removeFadeReady = false;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            radioCanvas = GameObject.Find("RadioCanvas").GetComponent<CanvasGroup>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerManager = player.GetComponent<PlayerManager>();
            //radioCanvas.alpha = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                AudioClip[] ac = new AudioClip[1];
                ac.SetValue(clipList[0], 0);
                SoundManager.PlaySound(ref ac, GetComponent<AudioSource>(), GetComponent<AudioSource>().volume / 3);

                mesh.enabled = false;
                fx.Play();
                aura.SetActive(false);
                isReadyToFade = true;
                StartCoroutine(nameof(TeleportPlayer));
            }
        }

        private void SpecialPower()
        {
            playerManager.HealthMaxValue = 200;
            playerManager.Health = 200;
            NotifyObservers(Events.SPECIALCRATE_PICKUP);
            NotifyObservers(Events.HEALTH);
        }

        private void Fading()
        {
            //if (isReadyToFade)
            //{
            //    Debug.Log("Entered ISREAADYFADE");
            //    Debug.Log("alpha: " + radioCanvas.alpha);
            //    if (radioCanvas.alpha < 1)
            //        radioCanvas.alpha += 0.50f * Time.deltaTime;
            //}
            //if (removeFadeReady)
            //{
            //    if (radioCanvas.alpha > 0)
            //        radioCanvas.alpha -= 0.50f * Time.deltaTime;
            //}
        }
        IEnumerator TeleportPlayer()
        {
            if (isReadyToFade)
            {
                while (radioCanvas.alpha < 0.98f)
                {
                    radioCanvas.alpha += 0.50f * Time.deltaTime;
                    yield return null;
                }
            }

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
      
            NotifyObservers(Core.Events.SECRET_ROOM_OUT);
            ObjectPooling.instance.WakeUpAllEnemies();

            isReadyToFade = true;
            removeFadeReady = false;

            SpecialPower();

            StopCoroutine(nameof(TeleportPlayer));
            gameObject.SetActive(false);
        }
    }
}
