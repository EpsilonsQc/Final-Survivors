using UnityEngine;

namespace Final_Survivors.Cam
{
    public class TargetFrameRate : MonoBehaviour
    {
        [Header("Target Frame Rate")]
        [SerializeField] private int frameRate = 60; // target frame rate

        private void Awake()
        {
           SetFrameRate();
        }

        private void SetFrameRate()
        {
            Application.targetFrameRate = frameRate; // Set the target frame rate
        }
    }
}
