using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Final_Survivors.Environment;

namespace Final_Survivors.UI.Intro
{
    public class Intro : MonoBehaviour
    {
        [Header("Canvas References")]
        [SerializeField] private GameObject HUD; // Reference to the HUD canvas
        [SerializeField] private GameObject cursorManager; // Reference to the cursor manager
        [SerializeField] private GameObject radioCanvas; // Reference to the radio canvas
        [SerializeField] private GameObject menuManager;

        [Header("Fade References")]
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        [Header("Parameters")]
        [SerializeField] private float fadeSpeed = 0.33f;

        // Fade In/Out
        private float startTime = default;
        private float endTime = default;

        // Game State
        private bool isGameStarted;

        // Player Input
        private PlayerInput playerInput;
        private InputAction skipIntroAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            skipIntroAction = playerInput.actions["Skip Intro"];
            radioCanvas.SetActive(false);
        }

        private void Update()
        {
            Skip(); // Skip the intro if the player presses the skip button

            if (image == null || text == null)
            {
                return;
            }

            if (!isGameStarted)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }

        private void FadeIn()
        {
            startTime += Time.unscaledDeltaTime * fadeSpeed;
            text.color = Color.Lerp(Color.clear, Color.white, startTime);

            if (startTime >= 2.0f)
            {
                isGameStarted = true;
            }
        }

        private void FadeOut()
        {
            endTime += Time.unscaledDeltaTime * fadeSpeed;
            text.color = Color.Lerp(Color.white, Color.clear, endTime);
            image.color = Color.Lerp(Color.black, Color.clear, endTime);

            if (text.color.a <= 0.75f && EnvironmentState.GetIsIntroduction())
            {
                ResumeGame();
            }

            if (text.color.a <= 0)
            {
                Disable();
            }
        }

        private void ResumeGame()
        {
            cursorManager.SetActive(true); // Enable the cursor manager
            Time.timeScale = 1f; // Resume the game
            EnvironmentState.SetIsPause(false); // Resume the game environment state
            EnvironmentState.SetIsIntroduction(false);
        }

        private void Disable()
        {
            this.gameObject.SetActive(false);
            HUD.SetActive(true);
            radioCanvas.SetActive(true);
        }

        private void Skip()
        {
            if (skipIntroAction.triggered)
            {
                Disable();
                ResumeGame();
            }
        }
    }
}
