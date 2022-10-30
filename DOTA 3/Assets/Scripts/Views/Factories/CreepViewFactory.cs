using Common.Enums;
using UnityEngine;
using Views.Abstracts;

namespace Views.Factories
{
    public class CreepViewFactory : ViewFactoryBase<CreepView>
    {
        [SerializeField] private CreepView _creepViewPrefab;
        [SerializeField] private Transform _creepContainer;
        public override CreepView Create()
        {
            var newCreep = Instantiate(_creepViewPrefab, _creepContainer);
            newCreep.transform.localScale = _creepViewPrefab.transform.localScale;
            return newCreep;
        }
    }
}