#if UNITY_EDITOR
using System.Linq;
using UnityEngine;

namespace Final_Survivors.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private bool isMusic = true;
        private float volume = 1f;
        private float length = 0f;
        private bool loop = true;
        private bool autoplay = true;
        private AudioSource currentAudioSource;
        private AudioSource audioSourceMusic;
        private AudioSource audioSourceFx;
        private SoundBank soundBank;
        private int selected = 13; // Music
        private string[] buttons = new string[14] {"dash", "walk", "run", "jump", "timeWarp", "laserSword", "handGun", "subMachineGun", "shotgun", "sniperRifle", "laserRifle", "electricRifle", "plasmaRifle", "music"};
        private AudioClip[] selectedAudioClip;

        private void Start()
        {
            audioSourceMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
            audioSourceFx = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
            currentAudioSource = audioSourceMusic;
            length = currentAudioSource.time;
            soundBank = FindObjectOfType<SoundBank>();
            selectedAudioClip = soundBank.audioClips.ElementAt(selected).Value;

            SoundManager.SelectMusicAudioClip(ref selectedAudioClip, currentAudioSource);

            if (autoplay)
                Play();
        }

        private void Update()
        {
            if (Mathf.Approximately(currentAudioSource.time, currentAudioSource.clip.length))
            {
                if (currentAudioSource.loop)
                {
                    currentAudioSource.time = 0;
                    Play();
                }
                else if (autoplay)
                {
                    SoundManager.ChangeMusic(ref selectedAudioClip, currentAudioSource, "next");
                }
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 200, 160, 1000));

            GUILayout.BeginVertical("Box");

            selected = GUILayout.SelectionGrid(selected, buttons, 1);

            if (GUILayout.Button("CHANGE AUDIO CLIP"))
            {
                AudioClipSelected(selected);
            }

            GUILayout.EndVertical();

            GUILayout.Space(5);

            GUILayout.BeginVertical("Box");

            if (GUILayout.Button("Play"))
            {
                Play();
            }

            GUILayout.Label("Track " + (SoundManager.GetCurrentMusicId() + 1) + " / " + selectedAudioClip.Length + " - " + Mathf.Ceil(currentAudioSource.time) + " / " + Mathf.Ceil(currentAudioSource.clip.length));
            length = GUILayout.HorizontalSlider(currentAudioSource.time, 0f, currentAudioSource.clip.length);

            if (! Mathf.Approximately(currentAudioSource.time, length))
            {
                SoundManager.SetMusicTime(currentAudioSource, length);
            }

            if (GUILayout.Button("Pause"))
            {
                SoundManager.PauseMusic(currentAudioSource);
            }

            if (GUILayout.Button("Stop"))
            {
                SoundManager.StopMusic(currentAudioSource);
            }

            if (GUILayout.Button("Previous"))
            {
                SoundManager.ChangeMusic(ref selectedAudioClip, currentAudioSource, "previous");
            }

            if (GUILayout.Button("Next"))
            {
                SoundManager.ChangeMusic(ref selectedAudioClip, currentAudioSource, "next");
            }

            if (GUILayout.Button("Random"))
            {
                SoundManager.ChooseRandomMusic(ref selectedAudioClip, currentAudioSource);
            }

            autoplay = GUILayout.Toggle(autoplay, " Autoplay");

            if (autoplay != SoundManager.GetAutoPlay())
            {
                SoundManager.AutoPlay(currentAudioSource, autoplay);
            }

            loop = GUILayout.Toggle(loop, " Loop");

            if (currentAudioSource.loop != loop)
            {
                SoundManager.LoopMusic(currentAudioSource, loop);
            }

            GUILayout.Label("Volume");
            volume = GUILayout.HorizontalSlider(volume, 0f, 1f);

            if (! Mathf.Approximately(currentAudioSource.volume, volume))
            {
                SoundManager.SetMusicVolume(currentAudioSource, volume);
            }

            GUILayout.EndVertical();

            GUILayout.EndArea();
        }

        private void Play()
        {
            if (isMusic)
            {
                SoundManager.PlayMusic(currentAudioSource, volume);
            }
            else
            {
                SoundManager.PlaySound(ref selectedAudioClip, currentAudioSource, volume);
            }
        }

        private void AudioClipSelected(int value)
        {
            selected = value;
            selectedAudioClip = soundBank.audioClips.ElementAt(selected).Value;

            if (soundBank.audioClips.ElementAt(selected).Key == "music")
                currentAudioSource = audioSourceMusic;
            else
                currentAudioSource = audioSourceFx;

            SoundManager.SelectMusicAudioClip(ref selectedAudioClip, currentAudioSource);
        }
    }
}
#endif
