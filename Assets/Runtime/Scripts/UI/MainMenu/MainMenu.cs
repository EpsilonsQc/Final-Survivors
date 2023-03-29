using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Final_Survivors.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject creditsMenu;
        [SerializeField] private GameObject audioMenu; // default options tab is "Audio" menu
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject difficultyMenu;
        [SerializeField] private GameObject difficultyMenuButtons; // special case
        [SerializeField] private GameObject cheatsMenu;

        [Header("Player Input")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputAction showHideMenuAction;

        private TextMeshProUGUI mainTitle;

        private void Awake()
        {
            //Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible

            mainTitle = GameObject.Find("Text/Logo").GetComponent<TextMeshProUGUI>();
            mainTitle.outlineWidth = 0.2f;
            mainTitle.outlineColor = Color.black;

            playerInput = GetComponent<PlayerInput>();
            showHideMenuAction = playerInput.actions["Toggle Pause Menu"];
        }

        private void OnEnable()
        {
            showHideMenuAction.performed += OnShowOrHidePauseMenu;
        }

        private void OnDisable()
        {
            showHideMenuAction.performed -= OnShowOrHidePauseMenu;
        }

        public void OnShowOrHidePauseMenu(InputAction.CallbackContext context)
        {
            if (creditsMenu.activeSelf == true || audioMenu.activeSelf == true || controlsMenu.activeSelf == true || difficultyMenu.activeSelf == true || cheatsMenu.activeSelf == true)
            {
                BackToPauseMenu(); // return to the pause menu
            }
        }

        public void Credits()
        {
            creditsMenu.SetActive(true);
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
            creditsMenu.SetActive(false);
            audioMenu.SetActive(false);
            controlsMenu.SetActive(false);
            difficultyMenu.SetActive(false);
            cheatsMenu.SetActive(false);
            difficultyMenuButtons.SetActive(false);
        }
    }
}