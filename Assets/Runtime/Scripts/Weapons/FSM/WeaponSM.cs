using Final_Survivors.Player;
using UnityEngine;

namespace Final_Survivors.Weapons
{
    public class WeaponSM : StateMachine
    {
        public PlayerController PController { get; private set; }
        public Animator Animator { get; private set; }
        public Weapon Weapon { get; private set; }

        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PController = player.GetComponent<PlayerController>();
            Animator = player.GetComponent<Animator>();
            Weapon = GetComponent<Weapon>();

            ChangeState(new IdleWeapon(this));
        }
    }
}
