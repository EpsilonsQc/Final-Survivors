using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Final_Survivors.Environment;

namespace Final_Survivors.UI.GameOverMenu
{
    public class GameOverMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject gameOverMenu;
        [SerializeField] private GameObject gameOverMenuButtons; 
        [SerializeField] private TextMeshProUGUI gameOverText;

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible
        }

        public void GameOver()
        {
            ShowCursor();
            Time.timeScale = 0f; // Pause the game
            EnvironmentState.SetIsPause(true);
            gameOverMenuButtons.SetActive(true);
            gameOverMenu.SetActive(true);

            gameOverText = GameObject.Find("Game Over/Text/Game Over").GetComponent<TextMeshProUGUI>();
            gameOverText.outlineWidth = 0.1f; // Set the outline width
            gameOverText.outlineColor = Color.black; // Set the outline color
        }

        public void Retry()
        {
            Time.timeScale = 1f; // Resume the game
            EnvironmentState.SetIsPause(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        }
    }
}