using System;

namespace Common.EventBus.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> subscriber) where T : IEvent;
        void Unsubscibe<T>(Action<T> subscriber) where T : IEvent;
        void Invoke<T>(T invocator) where T : IEvent;
        void Clear();
    }
}