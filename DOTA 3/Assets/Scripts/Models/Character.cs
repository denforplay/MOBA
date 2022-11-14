using System;
using System.Collections.Generic;
using System.Linq;
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
        public float CurrentDamage()
        {
            float currentDamage = Strength * BasePhysicalDamage;
            float additionalPercents = 0;
            
            for (int i = 0; i < _inventory.Items.Length; i++)
            {
                var currentItem = _inventory.Items[i];

                if (currentItem is not null)
                {
                    foreach (var valueConfig in currentItem.ValueConfigurations.Where(x => x.Characteristic == Characteristic.Damage))
                    {
                        switch (valueConfig.ItemValueType)
                        {
                            case ItemValueType.Value:
                            {
                                currentDamage += BasePhysicalDamage;
                                break;
                            }
                            case ItemValueType.Percentage:
                            {
                                additionalPercents += valueConfig.Value;
                                break;
                            }
                        }
                    }
                }
            }

            return currentDamage + currentDamage * additionalPercents;
        }

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
