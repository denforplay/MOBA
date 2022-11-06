using Common.Enums;
using UnityEngine;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;

namespace Views.Factories
{
    public class CreepViewFactory : ViewFactoryBase<CreepView, DefaultFactoryRequirement>
    {
        [SerializeField] private CreepView _creepViewPrefab;
        [SerializeField] private Transform _creepContainer;
        public override CreepView Create(DefaultFactoryRequirement requirement)
        {
            var newCreep = Instantiate(_creepViewPrefab, _creepContainer);
            newCreep.transform.localScale = _creepViewPrefab.transform.localScale;
            return newCreep;
        }
    }
}