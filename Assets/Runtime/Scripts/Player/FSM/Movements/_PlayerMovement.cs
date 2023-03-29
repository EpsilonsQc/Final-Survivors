namespace Final_Survivors.Player
{
    public abstract class _PlayerMovement : BaseState
    {
        protected _PlayerMovementSM _playerMovementSM;

        protected _PlayerMovement(_PlayerMovementSM _playerMovementSM)
        {
            this._playerMovementSM = _playerMovementSM;
        }
    }
}
