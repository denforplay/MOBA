using System;
using System.Collections.Generic;
using Common.EventBus.Interfaces;

namespace Common.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly IDictionary<Type, object> _eventDictionary = new Dictionary<Type, object>();

        public void Invoke<T>(T invokator) where T : IEvent
        {
            var type = invokator.GetType();

            if (!_eventDictionary.ContainsKey(type)) return;

            var action = _eventDictionary[type] as IEventAction<T>;

            action?.Invoke(invokator);
        }

        public void Clear()
        {
            _eventDictionary.Clear();
        }

        public void Subscribe<T>(Action<T> subscriber) where T : IEvent
        {
            if (!_eventDictionary.ContainsKey(typeof(T)))
            {
                _eventDictionary.Add(typeof(T), new EventAction<T>());
            }

            var action = _eventDictionary[typeof(T)] as EventAction<T>;
            action?.AddSubscriber(subscriber);
        }

        public void Unsubscibe<T>(Action<T> subscriber) where T : IEvent
        {
            if (!_eventDictionary.ContainsKey(typeof(T)))
            {
                return;
            }

            var action = _eventDictionary[typeof(T)] as IEventAction<T>;

            if (action == null || !action.HasSubscriber(subscriber))
            {
                return;
            }
            action.RemoveSubscriber(subscriber);
        }
    }
}