using Final_Survivors.Audio;
using Final_Survivors.Core;
using Final_Survivors.Observer;
using Final_Survivors.Projectile;
using Final_Survivors.Weapons;
using UnityEngine;

namespace Final_Survivors
{
    public class MasterProjectile : MonoBehaviour, IObserver
    {
        [SerializeField] private Mesh pistolM, machineGunM, shotgunM, sniperM, rocketM;
        [SerializeField] private Material pistolMat, machineGunMat, shotgunMat, sniperMat, rocketMat;
        [SerializeField] private Bullet pistol, machineGun, shotgun, sniper, rocket;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Bullet activeType;
        public SoundBank soundBank { get; private set; }
        public AudioSource source { get; private set; }
        public void SetupMaster()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            activeType = pistol;

            pistol.AddObserver(this);
            machineGun.AddObserver(this);
            shotgun.AddObserver(this);
            sniper.AddObserver(this);
            rocket.AddObserver(this);

            soundBank = FindObjectOfType<SoundBank>();
            source = GameObject.Find("FX").GetComponent<AudioSource>();
        }

        //private void OnEnable()
        //{
        //    pistol.AddObserver(this);
        //    machineGun.AddObserver(this);
        //    shotgun.AddObserver(this);
        //    sniper.AddObserver(this);
        //    rocket.AddObserver(this);
        //}
        //private void OnDisable()
        //{
        //    pistol.RemoveObserver(this);
        //    machineGun.RemoveObserver(this);
        //    shotgun.RemoveObserver(this);
        //    sniper.RemoveObserver(this);
        //    rocket.RemoveObserver(this);
        //}

        public void AddForce(Vector3 direction, Vector3 position)
        {
            activeType.AddForce(direction, position);
        }

        public void ActivateProjectile(WeaponType type)
        {        
            switch(type)
            {
                case WeaponType.PISTOL: activeType = pistol; activeType.enabled = true; meshFilter.mesh = pistolM; meshRenderer.material = pistolMat; break;
                case WeaponType.MACHINEGUN: activeType = machineGun; activeType.enabled = true; meshFilter.mesh = machineGunM; meshRenderer.material = machineGunMat; break;
                case WeaponType.SHOTGUN: activeType = shotgun; activeType.enabled = true; meshFilter.mesh = shotgunM; meshRenderer.material = shotgunMat; break;
                case WeaponType.SNIPER: activeType = sniper; activeType.enabled = true; meshFilter.mesh = sniperM; meshRenderer.material = sniperMat; break;
                case WeaponType.ROCKET_LAUNCHER: activeType = rocket; activeType.enabled = true; meshFilter.mesh = rocketM; meshRenderer.material = rocketMat; break;
            }
        }

        public void DeactivateProjectile()
        {
            activeType.enabled = false;
            ObjectPooling.instance.ReturnProjectilesToPool(this);
        }

        public void OnNotify(Events action)
        {
            if (action == Events.RETURN_BULLET)
            {
                DeactivateProjectile();
            }
        }
    }
}
