using Final_Survivors.Core;

namespace Final_Survivors.Player
{
    public class IdleAction : _PlayerAction
    {
        public IdleAction(_PlayerActionSM _playerActionSM) : base(_playerActionSM)
        {
        }

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            if (_playerActionSM.playerController.ActionTriggers[Events.MELEE])
            {
                _playerActionSM.ChangeState(new MeleeAttack(_playerActionSM));
            }
            else if (_playerActionSM.playerController.ActionTriggers[Events.SHOOT])
            {
                _playerActionSM.ChangeState(new RangedAttack(_playerActionSM));
            }
            else if (_playerActionSM.playerController.ActionTriggers[Events.TIME_WARP])
            {
                _playerActionSM.ChangeState(new TimeWarp(_playerActionSM));
            }
        }

        public override void OnExit()
        {
        }
    }
}
