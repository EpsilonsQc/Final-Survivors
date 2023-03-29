using UnityEngine;
using UnityEngine.InputSystem;

namespace Final_Survivors.Audio
{
    [RequireComponent(typeof(AudioSource))]

    public class PlayMusic : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioClip[] audioClip;
        private AudioSource audioSource;

        // User Input
        private UserInput userInput;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            userInput = new UserInput();

            userInput.AudioOptions.Play.performed += OnPlay;
            userInput.AudioOptions.Pause.performed += OnPause;
            userInput.AudioOptions.VolumeUp.performed += OnVolumeUp;
            userInput.AudioOptions.VolumeDown.performed += OnVolumeDown;
            userInput.AudioOptions.Previous.performed += OnPrevious;
            userInput.AudioOptions.Next.performed += OnNext;
        }

        private void Update()
        {
            if (Mathf.Approximately(audioSource.time, audioSource.clip.length))
            {
                Next();
            }
        }

        private void OnEnable()
        {
            userInput.AudioOptions.Enable();
        }

        private void OnDisable()
        {
            userInput.AudioOptions.Disable();
        }

        private void Start()
        {
            audioSource.clip = audioClip[0];
            audioSource.Play();
            //PlayRandomAudioClip();
        }

        private void PlayRandomAudioClip()
        {
            audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
            audioSource.Play();
        }

        private void OnPlay(InputAction.CallbackContext context)
        {
            audioSource.Play();
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            audioSource.Pause();
        }

        private void OnVolumeUp(InputAction.CallbackContext context)
        {
            audioSource.volume += 0.01f;
        }

        private void OnVolumeDown(InputAction.CallbackContext context)
        {
            audioSource.volume -= 0.01f;
        }

        private void OnPrevious(InputAction.CallbackContext context)
        {
            Previous();
        }

        private void OnNext(InputAction.CallbackContext context)
        {
            Next();
        }

        private void Previous()
        {
            if (audioSource.clip == audioClip[0])
            {
                audioSource.clip = audioClip[audioClip.Length - 1];
                audioSource.Play();
            }
            else
            {
                audioSource.clip = audioClip[System.Array.IndexOf(audioClip, audioSource.clip) - 1];
                audioSource.Play();
            }
        }

        private void Next()
        {
            if (audioSource.clip == audioClip[audioClip.Length - 1])
            {
                audioSource.clip = audioClip[0];
                audioSource.Play();
            }
            else
            {
                audioSource.clip = audioClip[System.Array.IndexOf(audioClip, audioSource.clip) + 1];
                audioSource.Play();
            }
        }
    }
}