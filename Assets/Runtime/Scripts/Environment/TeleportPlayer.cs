#if UNITY_EDITOR
using UnityEngine;

namespace Final_Survivors.Environment
{
    public class TeleportPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject[] teleporters;
        [SerializeField] private string[] names = new string[] { "SouthWest", "MiddleEast", "SecretRoom", "NorthEast" };
        private GameObject player;
        private int choiceIndex = -1;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void OnGUI()
        {
            choiceIndex = GUI.Toolbar(new Rect(50, 100, 350, 30), choiceIndex, names);
        }
        
        private void Update()
        {
            if (choiceIndex != -1)
            {
                player.transform.position = teleporters[choiceIndex].transform.position;
                choiceIndex = -1;
            }
        }
    }
}
#endif
