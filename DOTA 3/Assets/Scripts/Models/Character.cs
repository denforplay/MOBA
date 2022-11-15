using System;
using System.Collections.Generic;
using System.Linq;
using Common.Abstracts;
using Common.Enums;
using Configurations;
using Configurations.Character;
using Configurations.Levels;
using Models.Items;
using Models.Skills;

namespace Models
{
    public class Character : IHealthable, IManable, ILevelable
    {
        #region Health
        public event Action<float> OnHealthChanged;
        public event Action OnHealthEnded;
        public event Action<float> OnMaxHealthChanged;
        
        private float _currentHealth;
        private float _maxHealth;
        
        public float CurrentHealth => _currentHealth;
        
        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                OnMaxHealthChanged?.Invoke(_maxHealth);
            }
        }
        
        public void ChangeHealth(float value)
        {
            _currentHealth += value;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            
            OnHealthChanged?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
            {
                OnHealthEnded?.Invoke();
            }
        }
        #endregion

        #region Money
        public event Action<int> OnMoneyChanged; 
        
        private int _money;

        public int Money 
        { 
            get => _money;
            set
            {
                _money = value;
                OnMoneyChanged?.Invoke(_money);
            }
        }

        #endregion

        #region Mana

        private float _maxMana;
        private float _currentMana;
        public event Action<float> OnMaxManaChanged;
        public event Action<float> OnManaChanged;
        public float MaxMana => _maxMana;
        public float CurrentMana => _currentMana;
        public void ChangeMana(float value)
        {
            _currentMana += value;
            OnManaChanged?.Invoke(_currentMana);
        }
        
        #endregion

        #region Level-Experience

        private int _currentLevel;
        private float _currentExperience;
        private float _needForNextLevelExperience;
        private List<LevelConfiguration> _levels;
        
        public event Action<float> OnCurrentExperienceChanged;
        public event Action<float> OnNeedForNextLevelExperienceChanged; 
        public event Action<int> OnLevelChanged;
        public int CurrentLevel => _currentLevel;
        public float CurrentExperience => _currentExperience;
        public float NeedForNextLevelExperience => _needForNextLevelExperience;
        public void AddExperience(float value)
        {
            _currentExperience += value;
            if (_currentExperience >= NeedForNextLevelExperience)
            {
                if (CurrentLevel < _levels.Count)
                {
                    _currentLevel++;
                    OnLevelChanged?.Invoke(_currentLevel);

                }

                _currentExperience -= NeedForNextLevelExperience;
                _needForNextLevelExperience = _levels[_currentLevel - 1].ExperienceNeeded;
                OnNeedForNextLevelExperienceChanged?.Invoke(_needForNextLevelExperience);
            }
            
            OnCurrentExperienceChanged?.Invoke(_currentExperience);
        }

        private void InitializeLevels(LevelsConfiguration levelsConfiguration)
        {
            _levels = levelsConfiguration.LevelsConfigurations;
            _currentLevel = 1;
            _needForNextLevelExperience = _levels[_currentLevel - 1].ExperienceNeeded;
            _currentExperience = 0;
        }

        #endregion

        private List<ISkill> _skills;
        private Inventory _inventory;
        
        public Character(CharacterInfo characterInfo)
        {
            _maxHealth = characterInfo.MaxHealth;
            _currentHealth = _maxHealth;
            _maxMana = characterInfo.MaxMana;
            _currentMana = _maxMana;
            InitializeLevels(characterInfo.LevelsConfiguration);
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
       
        public Team Team { get; set; }
    }
}
