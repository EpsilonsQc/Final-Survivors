using System.Collections; // used for "IEnumerator"
using UnityEngine;
using UnityEngine.SceneManagement; // used for "SceneManager"
using UnityEngine.UI; // used for "Button" and "Image"
using TMPro; // used for "TextMeshProGUI"

namespace Final_Survivors.UI.MainMenu
{
    public class SceneLoader : MonoBehaviour
    {
        // References
        private GameObject mainMenuTexts;
        private GameObject mainMenuButtons;
        private Button startGameButton;
        private Image loadingBarImg;
        private Image loadingBarProgressImg;
        private TextMeshProUGUI startGameButtonText;
        private TextMeshProUGUI loadingPercentageText;
        private bool startLoadingScene = false;

        [Header("Loading Scene")]
        [SerializeField] private float changePerSecond = 50.0f;
        [SerializeField] private float loadingProgress = 0.0f;

        private void Awake()
        {
            mainMenuTexts = GameObject.Find("Main Menu/Text");
            mainMenuButtons = GameObject.Find("Main Menu/Buttons");

            startGameButton = GameObject.Find("Buttons/Start Game").GetComponent<Button>();
            startGameButton.onClick.AddListener(StartLoadingScene);

            loadingBarImg = GameObject.Find("Images/Loading Bar").GetComponent<Image>();
            loadingBarProgressImg = GameObject.Find("Images/Loading Bar/Loading Bar Progress").GetComponent<Image>();

            startGameButtonText = GameObject.Find("Buttons/Start Game/Text (Start Game)").GetComponent<TextMeshProUGUI>();
            loadingPercentageText = GameObject.Find("Images/Loading Bar/Text (Loading Percentage)").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (startLoadingScene == true && loadingProgress <= 100)
            {
                loadingBarImg.enabled = true;
                loadingBarProgressImg.enabled = true;

                loadingProgress += changePerSecond * Time.deltaTime;
                loadingPercentageText.text = ((int)loadingProgress + "%");

                loadingBarProgressImg.fillAmount = (float)loadingProgress / 100f;
            }
        }

        // Start loading scene
        private void StartLoadingScene()
        {
            StartCoroutine(LoadScene());
        }

        // Load scene (coroutine)
        private IEnumerator LoadScene()
        {
            if (!startLoadingScene)
            {
                mainMenuTexts.SetActive(false);
                mainMenuButtons.SetActive(false);

                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("01_Cyberpunk");
                asyncLoad.allowSceneActivation = false;

                while (!asyncLoad.isDone)
                {
                    startLoadingScene = true;

                    if(loadingProgress >= 100)
                    {
                        startGameButton.onClick.RemoveListener(StartLoadingScene);
                        asyncLoad.allowSceneActivation = true;
                    }
                    yield return null; // wait until the next frame before continuing
                }
            }
        }
    }
}