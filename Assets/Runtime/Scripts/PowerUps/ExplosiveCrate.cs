using UnityEngine;
using Final_Survivors.Weapons;
using Final_Survivors.Player;

namespace Final_Survivors.PowerUps
{
    public class ExplosiveCrate : Crate
    {
        int i = 0; // Debug variable to check how many times the explosive crate is picked up (should always be 1)

        public override void SetPowerUp()
        {          
            // Prevents the player from picking up an explosive more than once at the same time.
            i += 1;
            //Debug.Log("Explosive picked up " + i);

            if(i == 2)
            {
                //Debug.Log("ATTENTION : You have already picked up an explosive !");
                return;
            }

            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            int randomPick = Random.Range(0, 4);

            switch (randomPick)
            {
                case 0:
                    playerController.RandomPickWeapon(WeaponType.EXPLOSION);
                    //Debug.Log("Explosive1 chosen");
                    break;
                case 1:
                    //playerController.RandomPickWeapon(WeaponType.MACHINEGUN);
                    //Debug.Log("Explosive2 chosen");
                    break;
                case 2:
                    //playerController.RandomPickWeapon(WeaponType.MACHINEGUN);
                    //Debug.Log("Explosive2 chosen");
                    break;
            }

            DestroyCrate();
        }
    }
}