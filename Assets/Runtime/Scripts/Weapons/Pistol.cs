using Final_Survivors.Audio;
using UnityEngine;

namespace Final_Survivors.Weapons
{
    public class Pistol : Weapon
    {
        private Transform player;

        private void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        public override void Shoot()
        {
            if (isCycled)
            {
                ActivateFx(true);
                isCycled = false;
                cycleTimer = cycleTime;
                PlaySoundShoot();
                SpawnProjectile();
                InvokeRepeating(nameof(Cooldown), 0,Time.deltaTime);
            }
        }

        private void PlaySoundShoot()
        {
            SoundManager.PlaySound(ref soundBank.handGun, audioSource, audioSource.volume / 3);
        }

        private void SpawnProjectile()
        {
            MasterProjectile bullet = ObjectPooling.instance.TakeProjectilesFromPool(weaponType);
            bullet.AddForce(player.forward, transform.position);
        }

        private void Cooldown()
        {
            cycleTimer -= Time.deltaTime;

            if (cycleTimer < 0)
            {
                isCycled = true;
                CancelInvoke(nameof(Cooldown));
            }
            else if (cycleTimer < cycleTime/2) 
            {
                ActivateFx(false);
            }
        }
    }
}
