namespace Final_Survivors.Player
{
    public abstract class _PlayerAction : BaseState
    {
        protected _PlayerActionSM _playerActionSM;

        protected _PlayerAction(_PlayerActionSM _playerActionSM)
        {
            this._playerActionSM = _playerActionSM;
        }
    }
}
