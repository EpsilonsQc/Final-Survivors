namespace Final_Survivors.Weapons
{
    public abstract class WeaponBaseState : BaseState
    {
        protected WeaponSM weaponSM;

        public WeaponBaseState(WeaponSM weaponSM)
        {
            this.weaponSM = weaponSM;
        }
    }
}
