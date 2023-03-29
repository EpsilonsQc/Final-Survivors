using UnityEngine;

namespace Final_Survivors.Core
{
    public class CursorManager : MonoBehaviour
    {
        // Cursor is white by default. Cursor turn red when over enemy. Cursor turn green when over interactable.
        [Header("Cursor")]
        [SerializeField] Texture2D cursorTextureMenu = null;
        [SerializeField] Texture2D cursorTextureDefault = null;
        [SerializeField] Texture2D cursorTextureEnemy = null;
        [SerializeField] Texture2D cursorTextureInteractable = null;

        // Cursor hotspot is the center of the cursor
        [Header("Cursor Hotspot")]
        [SerializeField] Vector2 cursorHotspotMenu = new Vector2(75, 25);
        [SerializeField] Vector2 cursorHotspotTarget = new Vector2(128, 128);

        // Assign layer in the Inspector
        [Header("Layers")]
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] LayerMask interactableLayer;

        // Change cursor when game is in pause menu
        private bool isPausedCursor;

        public bool IsPausedCursor
        {
            set { isPausedCursor = value; }
        }

        private void Start()
        {
            Cursor.SetCursor(cursorTextureDefault, cursorHotspotTarget, CursorMode.Auto);
            Cursor.lockState= CursorLockMode.Confined;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(isPausedCursor)
            {
                Cursor.SetCursor(cursorTextureMenu, cursorHotspotMenu, CursorMode.Auto);
            }
            else if (Physics.Raycast(ray, out hit, float.MaxValue, enemyLayer))
            {
                Cursor.SetCursor(cursorTextureEnemy, cursorHotspotTarget, CursorMode.Auto);
            }
            else if (Physics.Raycast(ray, out hit, float.MaxValue, interactableLayer))
            {
                Cursor.SetCursor(cursorTextureInteractable, cursorHotspotTarget, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(cursorTextureDefault, cursorHotspotTarget, CursorMode.Auto);
            }
        }
    }
}