using Final_Survivors.Core;

namespace Final_Survivors.Player
{
    public class RangedAttack : _PlayerAction
    {

        public RangedAttack(_PlayerActionSM playerActionSM) : base(playerActionSM)
        {
        }

        public override void OnEnter()
        {
            _playerActionSM.Animator.SetBool("isAttackDistance", true);
        }

        public override void OnUpdate()
        {
            if (!_playerActionSM.playerController.ActionTriggers[Events.SHOOT])
            {
                if (_playerActionSM.playerController.ActionTriggers[Events.MELEE])
                {
                    _playerActionSM.ChangeState(new MeleeAttack(_playerActionSM));
                }
                else if (_playerActionSM.playerController.ActionTriggers[Events.TIME_WARP])
                {
                    _playerActionSM.ChangeState(new TimeWarp(_playerActionSM));
                }
                else
                {
                    _playerActionSM.ChangeState(new IdleAction(_playerActionSM));
                }
            }
            /*
            else
            {
                _playerActionSM.playerManager.RangedAttack();
            }
            */
        }

        public override void OnExit()
        {
            _playerActionSM.Animator.SetBool("isAttackDistance", false);
        }
    }
}
