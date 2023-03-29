using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

namespace Final_Survivors.UI.MainMenu
{
    public class PlayVideos : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject menuManager;

        [Header("Resources")]
        [SerializeField] private VideoClip[] videoClip;
        private VideoPlayer videoPlayer;

        private PlayerInput playerInput;
        private InputAction previousVideoAction;
        private InputAction nextVideoAction;

        private void Awake()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.clip = videoClip[0]; // start with the first video

            if(menuManager == null)
            {
                menuManager = GameObject.Find("Menu Manager");
            }

            playerInput = menuManager.GetComponent<PlayerInput>();
            previousVideoAction = playerInput.actions["Previous Video"];
            nextVideoAction = playerInput.actions["Next Video"];
        }

        private void OnEnable()
        {
            previousVideoAction.performed += OnPreviousVideo;
            nextVideoAction.performed += OnNextVideo;
        }

        private void OnDisable()
        {
            previousVideoAction.performed -= OnPreviousVideo;
            nextVideoAction.performed -= OnNextVideo;
        }

        public void OnPreviousVideo(InputAction.CallbackContext context)
        {
            if (videoPlayer.clip == videoClip[0])
            {
                videoPlayer.clip = videoClip[3];
            }
            else if (videoPlayer.clip == videoClip[1])
            {
                videoPlayer.clip = videoClip[0];
            }
            else if (videoPlayer.clip == videoClip[2])
            {
                videoPlayer.clip = videoClip[1];
            }
            else if (videoPlayer.clip == videoClip[3])
            {
                videoPlayer.clip = videoClip[2];
            }
        }

        public void OnNextVideo(InputAction.CallbackContext context)
        {
            if (videoPlayer.clip == videoClip[0])
            {
                videoPlayer.clip = videoClip[1];
            }
            else if (videoPlayer.clip == videoClip[1])
            {
                videoPlayer.clip = videoClip[2];
            }
            else if (videoPlayer.clip == videoClip[2])
            {
                videoPlayer.clip = videoClip[3];
            }
            else if (videoPlayer.clip == videoClip[3])
            {
                videoPlayer.clip = videoClip[0];
            }
        }
    }
}