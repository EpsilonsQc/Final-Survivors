using Final_Survivors.Audio;
using UnityEngine;

namespace Final_Survivors.Projectile
{
    public enum AmmoType { PISTOL_AMMO, MG_AMMO, SHOTGUN_AMMO, SNIPER_AMMO, ROCKET }

    public class Bullet : Projectile
    {
        private void OnEnable()
        {
            rb.velocity = Vector3.zero;
        }

        public virtual void AddForce(Vector3 direction, Vector3 position)
        {
            transform.position = position;
            transform.up = direction;
            rb.AddForce(projectileSpeed * direction, ForceMode.Impulse);
        }
    }
}
