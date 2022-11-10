using System.Collections.Generic;
using System.Linq;
using Common.Enums;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Creep configuration", order = 0)]
    public class CreepConfiguration : ScriptableObject
    {
        [SerializeField] private List<AnimationConfiguration> _creepAnimations;
        [SerializeField] private CombatType _combatType;
        [SerializeField] private float _observingRadius;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _attackDelay;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _damage;
        
        public CombatType CombatType => _combatType;
        public Dictionary<AnimationType, string> AnimationsInfo => _creepAnimations.Select(x => x.AnimationInfo).ToDictionary(x => x.Key, x => x.Value);
        public float ObservingRadius => _observingRadius;
        public float AttackDistance => _attackDistance;
        public float AttackDelay => _attackDelay;
        public float Damage => _damage;
        public float MaxHealth => _maxHealth;
    }
}