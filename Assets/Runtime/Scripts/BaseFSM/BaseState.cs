namespace Final_Survivors
{
    public abstract class BaseState
    {
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}
