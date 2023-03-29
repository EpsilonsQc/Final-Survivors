using UnityEngine;
using UnityEngine.Audio;

namespace Final_Survivors.Core
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;

        private void Start()
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float volume = PlayerPrefs.GetFloat("MasterVolume");
                AdjustVolume("MasterVolume", volume);
            }

            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float volume = PlayerPrefs.GetFloat("MusicVolume");
                AdjustVolume("MusicVolume", volume);
            }

            if (PlayerPrefs.HasKey("UIVolume"))
            {
                float volume = PlayerPrefs.GetFloat("UIVolume");
                AdjustVolume("UIVolume", volume);
            }

            if (PlayerPrefs.HasKey("FXVolume"))
            {
                float volume = PlayerPrefs.GetFloat("FXVolume");
                AdjustVolume("FXVolume", volume);
            }
        }

        private void AdjustVolume(string audioMixerParameterName, float volume)
        {
            // Debug.Log("Adjusting volume: " + audioMixerParameterName + " to " + volume);
            audioMixer.SetFloat(audioMixerParameterName, volume); // Set the volume of the mixer
            PlayerPrefs.SetFloat(audioMixerParameterName, volume); // Save the volume setting to PlayerPrefs
        }
    }
}
