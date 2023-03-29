using System.Collections;
using UnityEngine;
using Final_Survivors.Player;

namespace Final_Survivors.Audio
{
    public class PlayerSound : MonoBehaviour
    {
        [Header("Audio source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Audio clips")]
        [SerializeField] public AudioClip[] walkAudio;
        [SerializeField] public AudioClip[] dashAudio;
        
        private bool stepAudioIsPlaying = false;
        private PlayerManager playerMovement;

        private void Awake()
        {
            // playerMovement = FindObjectOfType<PlayerMovement>();
        }

        void Update()
        {
            PlayStepAudio();
        }

        private void PlayStepAudio()
        {
            // TODO: implement "is moving" check for player
            // if (playerMovement.IsMoving())
            {
                if (!stepAudioIsPlaying)
                {
                    // StartCoroutine(PlayStep(walkAudio[Random.Range(0, walkAudio.Length)]));
                }
            }
        }

        private IEnumerator PlayStep(AudioClip audioClip)
        {
            stepAudioIsPlaying = true;
            audioSource.PlayOneShot(audioClip, 0.1f);
            yield return new WaitForSecondsRealtime(0.5f);
            stepAudioIsPlaying = false;
        }

        public void Play(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}