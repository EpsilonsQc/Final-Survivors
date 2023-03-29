using UnityEngine;

namespace Final_Survivors.Audio
{
    public static class SoundManager
    {
        private static int currentMusicId = 0;
        private static bool autoPlay = false;

        public static int GetCurrentMusicId()
        {
            return currentMusicId;
        }

        public static bool GetAutoPlay()
        {
            return autoPlay;
        }

        public static void PlaySound(ref AudioClip[] audioClip, AudioSource audioSource, float volume)
        {
            audioSource.PlayOneShot(audioClip[Random.Range(0, audioClip.Length)], volume);
            float rand = Random.Range(0.95f, 1.05f);
            audioSource.pitch = rand;
        }

        public static void ChooseRandomMusic(ref AudioClip[] audioClip, AudioSource audioSource)
        {
            currentMusicId = Random.Range(0, audioClip.Length);
            audioSource.clip = audioClip[currentMusicId];
            audioSource.time = 0;

            if (autoPlay)
            {
                audioSource.Play();
            }
        }

        public static void SelectMusicAudioClip(ref AudioClip[] audioClip, AudioSource audioSource)
        {
            currentMusicId = 0;
            audioSource.clip = audioClip[currentMusicId];
            audioSource.time = 0;

            if (autoPlay)
            {
                audioSource.Play();
            }
        }

        public static void LoopMusic(AudioSource audioSource, bool value)
        {
            if (audioSource.loop != value)
            {
                audioSource.loop = value;
            }
        }

        public static void AutoPlay(AudioSource audioSource, bool value)
        {
            if (autoPlay != value)
            {
                autoPlay = value;
            }
        }

        public static void PlayMusic(AudioSource audioSource, float volume)
        {
            audioSource.volume = volume;
            audioSource.Play();
        }

        public static void StopMusic(AudioSource audioSource)
        {
            audioSource.Stop(); 
        }

        public static void PauseMusic(AudioSource audioSource)
        {
            audioSource.Pause();
        }

        public static void SetMusicVolume(AudioSource audioSource, float volume)
        {
            audioSource.volume = volume;
        }

        public static void SetMusicTime(AudioSource audioSource, float time)
        {
            if (time < audioSource.clip.length)
            {
                audioSource.time = time;
            }
        }

        public static void ChangeMusic(ref AudioClip[] audioClip, AudioSource audioSource, string command)
        {
            if (command == "previous")
            {
                if (currentMusicId > 0)
                {
                    --currentMusicId;
                }
                else
                {
                    currentMusicId = audioClip.Length - 1;
                }
            }
            else if (command == "next")
            {
                if (currentMusicId >= audioClip.Length - 1)
                {
                    currentMusicId = 0;
                }
                else
                {
                    ++currentMusicId;
                }
            }

            audioSource.clip = audioClip[currentMusicId];
            audioSource.time = 0;

            if (autoPlay)
            {
                audioSource.Play();
            }
        }
    }
}
