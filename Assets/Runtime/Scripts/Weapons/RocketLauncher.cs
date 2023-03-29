using Final_Survivors.Audio;
using UnityEngine;

namespace Final_Survivors.Weapons
{
    public class RocketLauncher : Weapon
    {
        [SerializeField] private Transform muzzle;
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
            if (isCycled && ammoCount > 0)
            {
                ActivateFx(true);
                muzzle.gameObject.SetActive(false);
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
            SoundManager.PlaySound(ref soundBank.sniperRifle, audioSource, audioSource.volume / 3);
        }

        private void SpawnProjectile()
        {
            MasterProjectile bullet = ObjectPooling.instance.TakeProjectilesFromPool(weaponType);
            bullet.transform.up = player.forward;
            bullet.AddForce(bullet.transform.up, muzzle.position);
        }

        private void Cooldown()
        {
            cycleTimer -= Time.deltaTime;

            if (cycleTimer <= 0)
            {
                muzzle.gameObject.SetActive(true);
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
