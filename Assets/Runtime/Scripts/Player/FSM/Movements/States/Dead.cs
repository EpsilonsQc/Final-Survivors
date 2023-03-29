using Final_Survivors.Core;
using UnityEngine;

namespace Final_Survivors.Player
{
    public class Dead : _PlayerMovement
    {
        public Dead(_PlayerMovementSM playerMovementSM) : base(playerMovementSM)
        {
        }

        public override void OnEnter()
        {
            _playerMovementSM.Animator.SetBool("isDead", true);
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            _playerMovementSM.Animator.SetBool("isDead", false);
        }

    }
}
