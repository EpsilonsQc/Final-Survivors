using Final_Survivors.Environment;
using UnityEngine;

namespace Final_Survivors.UI.CharacterSelectionMenu
{
    public class CharacterSelection : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject characterSelectionMenu;
        [SerializeField] private GameObject characterSelectionMenuButtons;
        [SerializeField] private GameObject HUD;
        [SerializeField] private Camera characterSelectorCamera;
        [SerializeField] private GameObject difficultyMenu;

        private GameObject skins, characterSkins;
        private int index = 6;

        private void Awake()
        {
            EnvironmentState.SetIsIntroduction(true);
        }

        private void Start()
        {
            HUD.SetActive(false);
            Time.timeScale = 0f; // Pause the game
            characterSelectionMenuButtons.SetActive(true);
            characterSelectionMenu.SetActive(true);
            EnvironmentState.SetIsPause(true);

            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible

            skins = GameObject.FindGameObjectWithTag("Skins");
            characterSkins = GameObject.FindGameObjectWithTag("CharacterSkins");
        }

        public void Select()
        {
            characterSelectorCamera.enabled = false;
            difficultyMenu.SetActive(true);
            characterSelectionMenu.SetActive(false);
        }

        public void NextSkin()
        {
            skins.transform.GetChild(index).gameObject.SetActive(false);
            characterSkins.transform.GetChild(index).gameObject.SetActive(false);

            if (index < skins.transform.childCount - 1)
                ++index;
            else
                index = 0;

            skins.transform.GetChild(index).gameObject.SetActive(true);
            characterSkins.transform.GetChild(index).gameObject.SetActive(true);
        }

        public void PreviousSkin()
        {
            skins.transform.GetChild(index).gameObject.SetActive(false);
            characterSkins.transform.GetChild(index).gameObject.SetActive(false);

            if (index > 0)
                --index;
            else
                index = skins.transform.childCount - 1;

            skins.transform.GetChild(index).gameObject.SetActive(true);
            characterSkins.transform.GetChild(index).gameObject.SetActive(true);
        }
    }
}