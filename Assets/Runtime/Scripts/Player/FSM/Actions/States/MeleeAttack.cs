using Final_Survivors.Core;

namespace Final_Survivors.Player
{
    public class MeleeAttack : _PlayerAction
    {
        public MeleeAttack(_PlayerActionSM _playerActionSM) : base(_playerActionSM)
        {
        }

        public override void OnEnter()
        {
            _playerActionSM.Animator.SetBool("isAttackCAC", true);
        }

        public override void OnUpdate()
        {
            if (!_playerActionSM.playerController.ActionTriggers[Events.MELEE])
            {
                if (_playerActionSM.playerController.ActionTriggers[Events.SHOOT])
                {
                    _playerActionSM.ChangeState(new RangedAttack(_playerActionSM));
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
                _playerActionSM.player.MeleeAttack();
            }
            */
        }

        public override void OnExit()
        {
            _playerActionSM.Animator.SetBool("isAttackCAC", false);
        }
    }
}
