using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using Final_Survivors.Core;
using Final_Survivors.Environment;

namespace Final_Survivors.UI.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject pauseMenuButtons; 
        [SerializeField] private GameObject HUD;
        [SerializeField] private CursorManager cursorManager; // Reference to CursorManager.cs

        private TextMeshProUGUI mainTitle;

        // Player Input
        private PlayerInput playerInput;

        // NEW OPTIONS MENU REFERENCES
        [Header("New Options Menu References")]
        [SerializeField] private GameObject audioMenu; // default options tab is "Audio" menu
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject difficultyMenu;
        [SerializeField] private GameObject cheatsMenu;

        // Difficulty Menu References (special case)
        [Header("Difficulty Menu References")]
        [SerializeField] private GameObject difficultyMenuButtons;

        [Header("Cheats Menu References")]
        [SerializeField] private GameObject Background;
        [SerializeField] private GameObject CheatsText;
        [SerializeField] private GameObject Buttons;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            pauseMenuButtons.SetActive(true);
        }

        private void OnEnable()
        {
            playerInput.actions["Toggle Pause Menu"].performed += OnShowOrHidePauseMenu;
        }

        private void OnDisable()
        {
            playerInput.actions["Toggle Pause Menu"].performed -= OnShowOrHidePauseMenu;
        }

        public void OnShowOrHidePauseMenu(InputAction.CallbackContext context)
        {
            if (cheatsMenu.activeSelf == true && pauseMenu.activeSelf == false) // In-game cheats menu edge case
            {
                CloseCheatsMenu(); // close the cheats menu
            }
            else if (pauseMenu.activeSelf == false)
            {
                Pause(); // open the pause menu
            }
            else if (pauseMenu.activeSelf == true)
            {
                if (audioMenu.activeSelf == false && controlsMenu.activeSelf == false && difficultyMenu.activeSelf == false && cheatsMenu.activeSelf == false)
                {
                    Resume(); // close the pause menu
                }
                else if (audioMenu.activeSelf == true || controlsMenu.activeSelf == true || difficultyMenu.activeSelf == true || cheatsMenu.activeSelf == true)
                {
                    BackToPauseMenu(); // return to the pause menu
                }
            }
        }

        public void Pause()
        {
            if (!EnvironmentState.GetIsIntroduction())
            {
                Time.timeScale = 0f; // Pause the game
                EnvironmentState.SetIsPause(true); // Pause the game (for the environment)
                cursorManager.IsPausedCursor = true;

                // Hide the HUD
                HUD.SetActive(false);

                // Hide cheats menu IN-GAME OVERLAY if open
                cheatsMenu.SetActive(false);
                Background.SetActive(true);
                CheatsText.SetActive(true);
                Buttons.SetActive(true);
                pauseMenu.SetActive(true);
                

                mainTitle = GameObject.Find("Text/Logo").GetComponent<TextMeshProUGUI>();
                mainTitle.outlineWidth = 0.1f;
                mainTitle.outlineColor = Color.black;
            }
        }

        public void Resume()
        {
            Time.timeScale = 1f; // Resume the game
            EnvironmentState.SetIsPause(false);
            cursorManager.IsPausedCursor = false;

            // Show the HUD
            HUD.SetActive(true);

            // Hide the pause menu
            audioMenu.SetActive(false);
            controlsMenu.SetActive(false);
            difficultyMenu.SetActive(false);
            cheatsMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f; // Resume the game
            SceneManager.LoadSceneAsync("00_MainMenu");
        }

        public void Quit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        // NEW OPTIONS MENU FUNCTIONS

        public void Options() // Audio is the default tab
        {
            audioMenu.SetActive(true); // default options tab is "Audio" menu
            controlsMenu.SetActive(false);
            difficultyMenu.SetActive(false);
            cheatsMenu.SetActive(false);
            difficultyMenuButtons.SetActive(false);
        }

        public void Controls()
        {
            audioMenu.SetActive(false);
            controlsMenu.SetActive(true);
            difficultyMenu.SetActive(false);
            cheatsMenu.SetActive(false);
            difficultyMenuButtons.SetActive(false);
        }

        public void Difficulty()
        {
            audioMenu.SetActive(false);
            controlsMenu.SetActive(false);
            difficultyMenu.SetActive(true);
            cheatsMenu.SetActive(false);
            difficultyMenuButtons.SetActive(true);
        }

        public void Cheats()
        {
            audioMenu.SetActive(false);
            controlsMenu.SetActive(false);
            difficultyMenu.SetActive(false);
            cheatsMenu.SetActive(true);
            difficultyMenuButtons.SetActive(false);
        }

        // BACK BUTTONS FOR OPTIONS MENU

        public void BackToPauseMenu()
        {
            audioMenu.SetActive(false);
            controlsMenu.SetActive(false);
            difficultyMenu.SetActive(false);
            cheatsMenu.SetActive(false);
            difficultyMenuButtons.SetActive(false);
        }

        // IN-GAME CHEATS MENU EDGE CASE
        public void CloseCheatsMenu()
        {
            // Resume the game
            Time.timeScale = 1f; // Resume the game
            EnvironmentState.SetIsPause(false); // Resume the game (for the environment)
            cursorManager.IsPausedCursor = false;

            // Hide the cheats menu
            cheatsMenu.SetActive(false);

            // Restore/show some UI elements
            Background.SetActive(true);
            CheatsText.SetActive(true);
            Buttons.SetActive(true);
        }
    }
}