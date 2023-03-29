using UnityEngine;
using System.Collections.Generic;
using Final_Survivors.Core;

namespace Final_Survivors.Observer
{
    public abstract class Subject : MonoBehaviour
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(Events action)
        {
            _observers.ForEach((_observers) =>
            {
                _observers.OnNotify(action);
            });
        }
    }
}
