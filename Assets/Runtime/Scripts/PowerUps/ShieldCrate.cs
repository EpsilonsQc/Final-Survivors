using Final_Survivors.Audio;
using UnityEngine;

namespace Final_Survivors.PowerUps
{
    public class ShieldCrate : Crate
    {
        private const float pickupShieldValue = 20; // 20% of shield (max 100%)
        //private int i = 0; // Debug variable to check how many times the shield is picked up (should always be 1)
        private AudioSource source;
        [SerializeField] private AudioClip clip;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        public override void SetPowerUp()
        {
            /* Prevents the player from picking up the shield more than once at the same time.
            //i += 1;
            //Debug.Log("Shield picked up " + i);

            if (i == 2)
            {
                //Debug.Log("ATTENTION : You have already picked up a shield !");
                return;
            }
            */

            AudioClip[] ac = new AudioClip[1];
            ac.SetValue(clip, 0);
            SoundManager.PlaySound(ref ac, source, source.volume / 3);

            if (ShieldManager.Instances.GetPlayerManager().ShieldValue + pickupShieldValue <= ShieldManager.Instances.GetPlayerManager().ShieldMaxValue)
            {
                ShieldManager.Instances.GetPlayerManager().ShieldValue += pickupShieldValue;
            }
            else
            {
                ShieldManager.Instances.GetPlayerManager().ShieldValue = ShieldManager.Instances.GetPlayerManager().ShieldMaxValue;
            }
        }
    }
}
