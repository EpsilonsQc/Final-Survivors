using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Final_Survivors.Environment;

namespace Final_Survivors.UI.DifficultyMenu
{
    public class DifficultyMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject difficultyMenu;
        [SerializeField] private GameObject difficultyMenuButtons;
        [SerializeField] private Button defaultDifficultyButton;
        [SerializeField] private GameObject selectButton;
        [SerializeField] private TextMeshProUGUI difficultyMenuText;
        [SerializeField] private GameObject pauseMenuButtons;
        [SerializeField] private GameObject introMenu;

        private SpawnManager spawnManager;
        private bool isInit = true;

        private void Awake()
        {
            spawnManager = GameObject.FindObjectOfType<SpawnManager>();
            defaultDifficultyButton.Select(); // The default difficulty is "selected" by default
        }

        private void OnDisable()
        {
            if (!isInit && selectButton.activeSelf)
            {
                selectButton.SetActive(false);
            }
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible
        }

        public void Difficulty()
        {
            pauseMenuButtons.SetActive(false);
            ShowCursor();

            Time.timeScale = 0f; // Pause the game
            EnvironmentState.SetIsPause(true);

            difficultyMenuButtons.SetActive(true);
            difficultyMenu.SetActive(true);

            difficultyMenuText.outlineWidth = 0.1f; // Set the outline width
            difficultyMenuText.outlineColor = Color.black; // Set the outline color
        }

        public void SelectDifficulty()
        {
            isInit = false;
            introMenu.SetActive(true);
            difficultyMenu.SetActive(false);
        }

        public void Back()
        {
            pauseMenuButtons.SetActive(true);
            difficultyMenu.SetActive(false);
        }

        public void SetDifficulty(int value)
        {
            spawnManager.Level = value;
        }
    }
}