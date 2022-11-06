using System.Collections.Generic;
using System.Linq;
using Common.Enums;
using UnityEngine;

namespace Configurations.Character
{
    [CreateAssetMenu(menuName = "Configurations/Character info", order = 0)]
    public class CharacterInfo : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private float _speed;
        [SerializeField] private List<SkillConfiguration> _skillConfigurations;
        [SerializeField] private CombatType _combatType;
        [SerializeField] private ProjectileType _projectileType;
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackDelay;
        [SerializeField] private List<AnimationConfiguration> _animations;
            
        private int Id => _id;
        public float Speed => _speed;
        public float AttackRange => _attackRange;
        public float AttackDelay => _attackDelay;
        public ProjectileType ProjectileType => _projectileType;
        public CombatType CombatType => _combatType;
        public Dictionary<AnimationType, string> AnimationsInfo => _animations.Select(x => x.AnimationInfo).ToDictionary(x => x.Key, x => x.Value);
        public List<SkillConfiguration> SkillConfigurations => _skillConfigurations;
    }
}