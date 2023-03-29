using Final_Survivors.Core;

namespace Final_Survivors.Player
{
    public class Dash : _PlayerMovement
    {
        public Dash(_PlayerMovementSM _playerMovementSM) : base(_playerMovementSM)
        {
        }

        public override void OnEnter()
        {
            _playerMovementSM.Animator.SetBool("isDash", true);
        }

        public override void OnUpdate()
        {
            if (!_playerMovementSM.PController.ActionTriggers[Events.DASH])
            {
                if (_playerMovementSM.PController.ActionTriggers[Events.MOVE])
                {
                    _playerMovementSM.ChangeState(new Move(_playerMovementSM));
                }
                else
                {
                    _playerMovementSM.ChangeState(new Idle(_playerMovementSM));
                }
            }
            else
            {
                _playerMovementSM.playerMovement.Dash(_playerMovementSM.moveInputValue);
            }
        }

        public override void OnExit()
        {
            _playerMovementSM.Animator.SetBool("isDash", false);
            _playerMovementSM.PController.ActionTriggers[Events.DASH] = false;
        }
    }
}
