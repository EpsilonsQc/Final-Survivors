using UnityEngine;
using UnityEngine.EventSystems;

namespace Final_Survivors.Player
{
    public class UIElementSound : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        [Header("Resources")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip clickSound;

        [Header("Settings")]
        [SerializeField] private bool enableHoverSound = true;
        [SerializeField] private bool enableClickSound = true;

        private void Awake()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableHoverSound == true && audioSource != null)
            {
                audioSource.PlayOneShot(hoverSound);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (enableClickSound == true && audioSource != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }
}