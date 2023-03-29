using Final_Survivors.Audio;
using UnityEngine;

namespace Final_Survivors.Weapons
{
    public class MachineGun : Weapon
    {
        private Transform player;

        private void Awake()
        {
            IncrementAmmo(ammoCapacity);

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }        
        }

        public override void Shoot()
        {
            // Debug.Log("Shooting Machine Gun");

            if (isCycled && ammoCount > 0)
            {
                // Debug.Log("Ammo left: " + ammoCount);
                ActivateFx(true);
                DecreaseAmmo(1);
                isCycled = false;
                cycleTimer = cycleTime;
                PlaySoundShoot();
                SpawnProjectile();
                InvokeRepeating("Cooldown", 0, Time.deltaTime);
            }
        }

        private void PlaySoundShoot()
        {
            SoundManager.PlaySound(ref soundBank.submachineGun, audioSource, audioSource.volume / 3);
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
                CancelInvoke("Cooldown");
            }
            else if (cycleTimer < cycleTime / 2)
            {
                ActivateFx(false);
            }
        }
    }
}
