using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Final_Survivors.Environment;

namespace Final_Survivors.UI.WinMenu
{
    public class WinMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject winMenu;
        [SerializeField] private GameObject winMenuButtons; 
        [SerializeField] private TextMeshProUGUI winText;

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
            winMenuButtons.SetActive(true);
            winMenu.SetActive(true);

            winText = GameObject.Find("Win Game/Text/You Won").GetComponent<TextMeshProUGUI>();
            winText.outlineWidth = 0.1f; // Set the outline width
            winText.outlineColor = Color.black; // Set the outline color
        }

        public void Retry()
        {
            Time.timeScale = 1f; // Resume the game
            EnvironmentState.SetIsPause(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        }
    }
}