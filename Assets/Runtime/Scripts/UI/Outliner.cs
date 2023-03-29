using UnityEngine;
using TMPro;

namespace Final_Survivors.UI
{
    public class Outliner : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI titleText;

        private void Awake()
        {
            titleText.outlineWidth = 0.2f;
            titleText.outlineColor = Color.black;
        }
    }
}
