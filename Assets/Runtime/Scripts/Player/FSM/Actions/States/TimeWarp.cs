using Final_Survivors.Core;

namespace Final_Survivors.Player
{
    public class TimeWarp : _PlayerAction
    {
        public TimeWarp(_PlayerActionSM playerActionSM) : base(playerActionSM)
        {
        }

        public override void OnEnter()
        {
            //_playerActionSM.player.TimeWarp();
            _playerActionSM.Animator.SetBool("isTimeWarp", true);
            _playerActionSM.playerManager.PlaySoundTimeWarp();
        }

        public override void OnUpdate()
        {
            if (!_playerActionSM.playerController.ActionTriggers[Events.TIME_WARP])
            {
                _playerActionSM.ChangeState(new IdleAction(_playerActionSM));
            }
        }

        public override void OnExit()
        {
            _playerActionSM.Animator.SetBool("isTimeWarp", false);
        }
    }
}
