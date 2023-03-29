using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Final_Survivors.Audio
{
    public class AudioMixerSlider : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string audioMixerParameterName;
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            SetSlider();
        }

        private void SetSlider()
        {
            slider.minValue = -40; // -40dB is the minimum volume
            slider.maxValue = 0; // 0dB is the maximum volume
            slider.value = PlayerPrefs.GetFloat(audioMixerParameterName, 0); // Set the slider to the current volume level
            slider.onValueChanged.AddListener(AdjustVolume); // Add a listener to the slider's onValueChanged event
        }

        private void AdjustVolume(float volume)
        {
            audioMixer.SetFloat(audioMixerParameterName, volume); // Set the volume of the mixer
            PlayerPrefs.SetFloat(audioMixerParameterName, volume); // Save the volume setting to PlayerPrefs
        }
    }
}