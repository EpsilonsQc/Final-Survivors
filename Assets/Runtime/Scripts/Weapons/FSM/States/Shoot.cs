using Final_Survivors.Core;

namespace Final_Survivors.Weapons
{
    public class Shoot : WeaponBaseState
    {
        public Shoot(WeaponSM weaponSM) : base(weaponSM) 
        { 
        
        }

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            if (!weaponSM.PController.ActionTriggers[Events.SHOOT])
            {
                weaponSM.ChangeState(new IdleWeapon(weaponSM));
            }

            weaponSM.Weapon.Shoot();
        }

        public override void OnExit() 
        {
        }
    }
}
