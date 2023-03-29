using Final_Survivors.Core;

namespace Final_Survivors.Weapons
{
    public class IdleWeapon : WeaponBaseState
    {
        public IdleWeapon(WeaponSM weaponSM): base(weaponSM)
        {

        }

        public override void OnEnter()
        {
        }
        public override void OnUpdate()
        {
            if (weaponSM.PController.ActionTriggers[Events.SHOOT])
            {
                weaponSM.ChangeState(new Shoot(weaponSM));
            }
        }
        public override void OnExit()
        {
        }
    }
}
