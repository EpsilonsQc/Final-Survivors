using Final_Survivors.Audio;
using Final_Survivors.Observer;
using Final_Survivors.Core;
using UnityEngine;
using Final_Survivors.Player;

namespace Final_Survivors.Weapons
{
    public enum WeaponType {PISTOL, MACHINEGUN, SHOTGUN, SNIPER, ROCKET_LAUNCHER, EXPLOSION, SWORD};

    public class Weapon : Subject, IObserver
    {
        private Subject _playerControllerSubject;
        [SerializeField] protected WeaponType weaponType;
        [SerializeField] protected float cycleTime;
        protected float cycleTimer = 0f;
        [SerializeField] protected float ammoCapacity;
        protected float ammoCount = 0f;
        protected SoundBank soundBank;
        protected AudioSource audioSource;
        protected bool isCycled = true;
        [SerializeField]private ParticleSystem[] particlesSystems;

        private void Start()
        {
            soundBank = FindObjectOfType<SoundBank>();
            audioSource = GetComponent<AudioSource>();
            _playerControllerSubject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            if (weaponType != WeaponType.SWORD && weaponType != WeaponType.PISTOL)
            {
                _playerControllerSubject.AddObserver(this);
            }
        }

        public void ActivateFx(bool value)
        {          
            if (value)
            {
                foreach (ParticleSystem ps in particlesSystems)
                {
                    ps.Play();
                }
            }
            else
            {
                foreach (ParticleSystem ps in particlesSystems)
                {
                    ps.Stop();
                }
            }
        }

        /*
        private void OnEnable()
        {
            if (weaponType != WeaponType.SWORD && weaponType != WeaponType.PISTOL)
            {
                _playerControllerSubject.AddObserver(this);
            }
        }

        private void OnDisable()
        {
            if (weaponType != WeaponType.SWORD && weaponType != WeaponType.PISTOL)
            {
                _playerControllerSubject.RemoveObserver(this);
            }
        }
        */

        public void OnNotify(Events action)
        {
            if (action == Events.RELOAD)
            {
                if (weaponType != WeaponType.SWORD && weaponType != WeaponType.PISTOL)
                {
                    RefillAmmo();
                }
            }
        }

        public virtual void Shoot() {}

        protected void DecreaseAmmo(float value)
        {
            if (ammoCount >= value)
            {
                ammoCount -= value;
            }
            else
            {
                ammoCount = 0f;
            }

            NotifyObservers(Events.AMMO);
        }

        protected void IncrementAmmo(float value)
        {
            ammoCount = value;
            NotifyObservers(Events.AMMO);
        }

        public float GetAmmoCapacity()
        {
            return ammoCapacity;
        }

        public float GetAmmoCount()
        {
            return ammoCount;
        }

        public void RefillAmmo()
        {
            IncrementAmmo(ammoCapacity);
        }
    }
}
