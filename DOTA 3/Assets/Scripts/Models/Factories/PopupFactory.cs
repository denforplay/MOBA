using Common.PopupSystem;
using UnityEngine;
using Zenject;

namespace Models.Factories
{
    public class PopupFactory<T> : PlaceholderFactory<UnityEngine.Object, Transform, T> where T : Popup
    {
        readonly DiContainer _container;

        public PopupFactory(DiContainer container)
        {
            _container = container;
        }

        public override T Create(Object param1, Transform param2)
        {
            return _container.InstantiatePrefabForComponent<T>(param1, param2);
        }
    }
}