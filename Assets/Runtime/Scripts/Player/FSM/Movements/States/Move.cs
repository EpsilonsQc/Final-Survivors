using Final_Survivors.Core;
using UnityEngine;

namespace Final_Survivors.Player
{
    public class Move : _PlayerMovement
    {
        public Move(_PlayerMovementSM playerMovementSM) : base(playerMovementSM)
        {
        }

        public override void OnEnter()
        {
            _playerMovementSM.Animator.SetBool("isMove", true);
            InitSubStates();
        }

        public override void OnUpdate()
        {
            if (_playerMovementSM.PController.ActionTriggers[Events.DASH])
            {
                _playerMovementSM.ChangeState(new Dash(_playerMovementSM));
            }
            else if (!_playerMovementSM.PController.ActionTriggers[Events.MOVE])
            {
                _playerMovementSM.ChangeState(new Idle(_playerMovementSM));
            }
            else 
            {
                _playerMovementSM.playerMovement.Move(_playerMovementSM.moveInputValue);

                InitSubStates();

                float movingAngle = Vector3.SignedAngle(_playerMovementSM.PController.transform.forward, _playerMovementSM.playerMovement.rb.velocity.normalized, _playerMovementSM.PController.transform.up);

                if (movingAngle > 144 && movingAngle <= 180 || movingAngle < -144 && movingAngle >= -180)
                {
                    _playerMovementSM.Animator.SetBool("isBackward", true);
                }
                else if (movingAngle > 108 && movingAngle <= 144)
                {
                    _playerMovementSM.Animator.SetBool("isDiagonalBackRight", true);
                }
                else if (movingAngle < -108 && movingAngle >= -144)
                {
                    _playerMovementSM.Animator.SetBool("isDiagonalBackLeft", true);
                }
                else if (movingAngle > 72 && movingAngle <= 108)
                {
                    _playerMovementSM.Animator.SetBool("isStrafeRight", true);
                }
                else if (movingAngle < -72 && movingAngle >= -108)
                {
                    _playerMovementSM.Animator.SetBool("isStrafeLeft", true);
                }
                else if (movingAngle > 36 && movingAngle <= 72)
                {
                    _playerMovementSM.Animator.SetBool("isDiagonalFrontRight", true);
                }
                else if (movingAngle < -36 && movingAngle >= -72)
                {
                    _playerMovementSM.Animator.SetBool("isDiagonalFrontLeft", true);
                }
                else if (movingAngle <= 36 && movingAngle >= 0 || movingAngle >= -36 && movingAngle <= 0)
                {
                    _playerMovementSM.Animator.SetBool("isForward", true);
                }
            }
        }

        private void InitSubStates()
        {
            _playerMovementSM.Animator.SetBool("isBackward", false);
            _playerMovementSM.Animator.SetBool("isForward", false);
            _playerMovementSM.Animator.SetBool("isDiagonalBackRight", false);
            _playerMovementSM.Animator.SetBool("isDiagonalBackLeft", false);
            _playerMovementSM.Animator.SetBool("isStrafeRight", false);
            _playerMovementSM.Animator.SetBool("isStrafeLeft", false);
            _playerMovementSM.Animator.SetBool("isDiagonalFrontRight", false);
            _playerMovementSM.Animator.SetBool("isDiagonalFrontLeft", false);
            _playerMovementSM.Animator.SetBool("isForward", false);
        }

        public override void OnExit()
        {
            InitSubStates();
            _playerMovementSM.Animator.SetBool("isMove", false);
        }
    }
}
