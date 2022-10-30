using Common.Abstracts;
using UnityEngine;

namespace Views.Abstracts
{
    public abstract class ViewFactoryBase<T> : MonoBehaviour, IFactory<T> where T : MonoBehaviour
    {
        public abstract T Create();

        public T CreateOnPosition(Vector3 position)
        {
            var view = Create();
            view.transform.position = position;
            return view;
        }
    }
}