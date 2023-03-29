using Final_Survivors.Core;

namespace Final_Survivors.Player
{
    public class Tbag : _PlayerMovement
    {
        public Tbag(_PlayerMovementSM playerMovementSM) : base(playerMovementSM)
        {

        }

        public override void OnEnter()
        {
            _playerMovementSM.Animator.SetBool("tBag", true);
        }

        public override void OnUpdate()
        {
            if (_playerMovementSM.PController.ActionTriggers[Events.DASH])
            {
                _playerMovementSM.ChangeState(new Dash(_playerMovementSM));
            }
            else if (_playerMovementSM.PController.ActionTriggers[Events.MOVE])
            {
                _playerMovementSM.ChangeState(new Move(_playerMovementSM));
            }
            else if (!_playerMovementSM.PController.ActionTriggers[Events.TBAG])
            {
                _playerMovementSM.ChangeState(new Idle(_playerMovementSM));
            }
        }

        public override void OnExit()
        {
            _playerMovementSM.Animator.SetBool("tBag", false);
        }
    }
}
