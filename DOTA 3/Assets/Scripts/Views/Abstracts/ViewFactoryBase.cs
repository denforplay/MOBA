using Common.Abstracts;
using UnityEngine;
using Views.Abstracts.FactoryRequirements;

namespace Views.Abstracts
{
    public abstract class ViewFactoryBase<T, TRequirement> : MonoBehaviour, IFactory<T, TRequirement> where T : MonoBehaviour
    where TRequirement : IFactoryRequirement
    {
        public abstract T Create(TRequirement requirement);

        public T CreateOnPosition(Vector3 position, TRequirement requirement)
        {
            var view = Create(requirement);
            view.transform.position = position;
            return view;
        }
    }
}