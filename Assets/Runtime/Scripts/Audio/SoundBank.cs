using System.Collections.Generic;
using UnityEngine;

namespace Final_Survivors.Audio
{
    public class SoundBank : MonoBehaviour
    {
        public Dictionary<string, AudioClip[]> audioClips = new Dictionary<string, AudioClip[]>();
        public AudioClip[] dash;
        public AudioClip[] walk;
        public AudioClip[] run;
        public AudioClip[] jump;
        public AudioClip[] timeWarp;
        public AudioClip[] laserSword;
        public AudioClip[] handGun;
        public AudioClip[] submachineGun;
        public AudioClip[] shotgun;
        public AudioClip[] shotgunPump;
        public AudioClip[] sniperRifle;
        public AudioClip[] laserRifle;
        public AudioClip[] electricRifle;
        public AudioClip[] plasmaRifle;
        public AudioClip[] music;
        public AudioClip[] crates;
        public AudioClip[] secretRoom;
        public AudioClip[] enemyHitBallistic;
        public AudioClip[] enemyHitSword;
        public AudioClip[] enemyHitExplosion;
        public AudioClip[] spiderQueenScream;

        private void Awake()
        {
            SetupList();
        }

        private void SetupList()
        {
            audioClips.Add("dash", dash);
            audioClips.Add("walk", walk);
            audioClips.Add("run", run);
            audioClips.Add("jump", jump);
            audioClips.Add("timewarp", timeWarp);
            audioClips.Add("laserSword", laserSword);
            audioClips.Add("handGun", handGun);
            audioClips.Add("submachineGun", submachineGun);
            audioClips.Add("shotgun", shotgun);
            audioClips.Add("sniperRifle", sniperRifle);
            audioClips.Add("laserRifle", laserRifle);
            audioClips.Add("electricRifle", electricRifle);
            audioClips.Add("plasmaRifle", plasmaRifle);
            audioClips.Add("music", music);
            audioClips.Add("crates", crates);
            audioClips.Add("secretRoom", secretRoom);
            audioClips.Add("enemyHitBallistic", enemyHitBallistic);
            audioClips.Add("enemyHitSword", enemyHitSword);
            audioClips.Add("enemyHitExplosion", enemyHitExplosion);
        }
    }
}
