using System.Collections.Generic;
using System.Linq;
using Common.Enums;
using Configurations.Levels;
using UnityEngine;

namespace Configurations.Character
{
    [CreateAssetMenu(menuName = "Configurations/Character info", order = 0)]
    public class CharacterInfo : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private float _speed;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _basePhysicalDamage;
        [SerializeField] private float _baseMagicalDamage;
        [SerializeField] private float _baseStrength;
        [SerializeField] private float _baseAgility;
        [SerializeField] private float _baseIntelligence;
        [SerializeField] private List<SkillConfiguration> _skillConfigurations;
        [SerializeField] private CombatType _combatType;
        [SerializeField] private ProjectileType _projectileType;
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackDelay;
        [SerializeField] private List<AnimationConfiguration> _animations;
        [SerializeField] private int _inventorySize;
        [SerializeField] private float _maxMana;
        [SerializeField] private LevelsConfiguration _levelsConfiguration;
            
        private int Id => _id;
        public float Speed => _speed;
        public float MaxHealth => _maxHealth;
        public float BasePhysicalDamage => _basePhysicalDamage;
        public float BaseMagicalDamage => _baseMagicalDamage;
        public float BaseStrength => _baseStrength;
        public float BaseIntelligence => _baseIntelligence;
        public float BaseAgility => _baseAgility;
        public float AttackRange => _attackRange;
        public float AttackDelay => _attackDelay;
        public ProjectileType ProjectileType => _projectileType;
        public CombatType CombatType => _combatType;
        public Dictionary<AnimationType, string> AnimationsInfo => _animations.Select(x => x.AnimationInfo).ToDictionary(x => x.Key, x => x.Value);
        public List<SkillConfiguration> SkillConfigurations => _skillConfigurations;
        public int InventorySize => _inventorySize;
        public float MaxMana => _maxMana;
        public LevelsConfiguration LevelsConfiguration => _levelsConfiguration;
    }
}