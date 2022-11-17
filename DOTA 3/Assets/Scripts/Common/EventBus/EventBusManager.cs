using System;
using Common.EventBus.Interfaces;

namespace Common.EventBus
{
    public class EventBusManager : Singleton<EventBusManager>
    {
        private IEventBus _eventBus;
        private new void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            _eventBus = new EventBus();
        }
        public void Subscribe<T>(Action<T> subscriber) where T : IEvent
        {
            GetInstance._eventBus.Subscribe<T>(subscriber);
        }

        public void Clear()
        {
            _eventBus.Clear();
        }

        public void Unsubscribe<T>(Action<T> subscriber) where T : IEvent
        {
            GetInstance._eventBus.Unsubscibe<T>(subscriber);
        }

        public void Invoke<T>(T invokator) where T : IEvent
        {
            GetInstance._eventBus.Invoke(invokator);
        }
    }
}