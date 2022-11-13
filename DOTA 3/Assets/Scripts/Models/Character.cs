using System;
using System.Collections.Generic;
using Common.Abstracts;
using Common.Enums;
using Configurations;
using Configurations.Character;
using Models.Items;
using Models.Skills;

namespace Models
{
    public class Character : IHealthable
    {
        public event Action<int> OnMoneyChanged; 

        private float _currentHealth;
        private float _maxHealth;
        private int _money;
        private List<ISkill> _skills;
        private Inventory _inventory;
        
        public Character(CharacterInfo characterInfo)
        {
            _maxHealth = characterInfo.MaxHealth;
            _currentHealth = _maxHealth;
            Speed = characterInfo.Speed;
            InitializeSkills(characterInfo.SkillConfigurations);
            BasePhysicalDamage = characterInfo.BasePhysicalDamage;
            Strength = characterInfo.BaseStrength;
            Agility = characterInfo.BaseAgility;
            Intelligence = characterInfo.BaseIntelligence;
            AttackDelay = characterInfo.AttackDelay;
            AttackRange = characterInfo.AttackRange;
            _inventory = new Inventory(characterInfo.InventorySize);
        }

        private void InitializeSkills(List<SkillConfiguration> skillConfigurations)
        {
            _skills = new List<ISkill>();

            foreach (var skill in skillConfigurations)
            {
                switch (skill.SkillType)
                {
                    case SkillType.Directed:
                    {
                        _skills.Add(new DirectedSkill(skill.Id, 0));
                        continue;
                    }
                    case SkillType.RangeTargetZone:
                    {
                        _skills.Add(new RangeTargetZoneSkill(skill.Id, 0));
                        continue;
                    }
                }
            }
        }

        public int Money 
        { 
            get => _money;
            set
            {
                _money = value;
                OnMoneyChanged?.Invoke(_money);
            }
            
        }

        public Inventory Inventory => _inventory;
        public List<ISkill> Skills => _skills;
        public float BasePhysicalDamage { get; set; }
        public float Speed { get; set; }
        public float Strength { get; set; }
        public float Agility { get; set; }
        public float Intelligence { get; set; }
        public float AttackDelay { get; set; }
        public float AttackRange { get; set; }
        public float CurrentDamage => Strength * BasePhysicalDamage;
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public Team Team { get; set; }
        public event Action<float> OnHealthChanged;
        public event Action OnHealthEnded;
        public void ChangeHealth(float value)
        {
            _currentHealth += value;
            OnHealthChanged?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
            {
                OnHealthEnded?.Invoke();
            }
        }
    }
}
