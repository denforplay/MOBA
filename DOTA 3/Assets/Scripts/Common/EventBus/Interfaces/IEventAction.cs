using System;

namespace Common.EventBus.Interfaces
{
    public interface IEventAction<T> where T : IEvent
    {
        void AddSubscriber(Action<T> subscriber);
        void RemoveSubscriber(Action<T> subscriber);
        bool HasSubscriber(Action<T> subscriber);
        void Invoke(T invokator);
    }
}