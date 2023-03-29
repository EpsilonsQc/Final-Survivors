using Final_Survivors.Core;

namespace Final_Survivors.Observer
{
    public interface IObserver
    {
        public void OnNotify(Events action);
    }
}
