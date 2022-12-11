using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Common.Abstracts;
using Common.Abstracts;
using Common.Enums;
using Configurations;
using Configurations.Levels;
using Cysharp.Threading.Tasks;
using Models.Items;
using Models.Skills.Skills;
using UnityEngine;
using UnityEngine.AI;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Models
{
    public class Character : IHealthable, IManable, ILevelable, IMoneyable, ICostable
    {
        private CancellationTokenSource _cancellationToken;
        
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
        private float _regenerateManaPerSecond;
        public event Action<float> OnMaxManaChanged;
        public event Action<float> OnManaChanged;

        public float MaxMana
        {
            get
            {
                var maxManaWithItem = _maxMana;
                
                for (int i = 0; i < _inventory.Items.Length; i++)
                {
                    var currentItem = _inventory.Items[i];

                    if (currentItem is not null)
                    {
                        foreach (var valueConfig in currentItem.ValueConfigurations.Where(x => x.Characteristic == Characteristic.Mana))
                        {
                            switch (valueConfig.ItemValueType)
                            {
                                case ItemValueType.Value:
                                {
                                    maxManaWithItem += valueConfig.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
                
                SetMaxMana(maxManaWithItem);
                return maxManaWithItem;
            }
        }

        public float CurrentMana => _currentMana;
        public void ChangeMana(float value)
        {
            _currentMana += value;
            OnManaChanged?.Invoke(_currentMana);
        }

        private void SetMaxMana(float value)
        {
            OnMaxManaChanged?.Invoke(value);
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

        #region BaseParameters

        private int _strength;
        private int _intelligence;
        private float _defense;
        public event Action<int> OnStrengthChanged;
        public event Action<int> OnIntelligenceChanged;
        public event Action<int> OnDefenseChanged;

        public int Strength
        {
            get => _strength;
            set
            {
                _strength = value;
                OnStrengthChanged?.Invoke(_strength);
            }
        }
        public int Intelligence
        {
            get => _intelligence;
            set
            {
                _intelligence = value;
                OnIntelligenceChanged?.Invoke(_intelligence);
            }
        }
        
        public float Defense
        {
            get
            {
                var currentDefense = _defense;
                var additionalPercents = 0f;


                for (int i = 0; i < _inventory.Items.Length; i++)
                {
                    var currentItem = _inventory.Items[i];

                    if (currentItem is not null)
                    {
                        foreach (var valueConfig in currentItem.ValueConfigurations.Where(x => x.Characteristic == Characteristic.Defense))
                        {
                            switch (valueConfig.ItemValueType)
                            {
                                case ItemValueType.Value:
                                {
                                    currentDefense += valueConfig.Value;
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

                return currentDefense;
            }
            set
            {
                _defense = value;
                OnDefenseChanged?.Invoke(_intelligence);
            }
        }

        #endregion

        private List<ISkill> _skills;
        private Inventory _inventory;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly int _cost;

        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        public Character(CharacterInfo characterInfo, NavMeshAgent navMeshAgent, Camera camera)
        {
            _cost = characterInfo.CharacterCost;
            _cancellationToken = new CancellationTokenSource();
            _regenerateManaPerSecond = characterInfo.RegenerateManaPerSecond;
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
            Defense = characterInfo.BaseDefense;
            AttackDelay = characterInfo.AttackDelay;
            AttackRange = characterInfo.AttackRange;
            _inventory = new Inventory(characterInfo.InventorySize);
            _navMeshAgent = navMeshAgent;
            UniTask.Create(RegenerateMana);
        }

        private void InitializeSkills(List<SkillConfiguration> skillConfigurations)
        {
            _skills = new List<ISkill>();

            foreach (var skill in skillConfigurations)
            {
                switch (skill.SkillType)
                {
                    case SkillType.Dash:
                    {
                        _skills.Add(new DashSkill(skill.Id, skill, this));
                        continue;
                    }
                    case SkillType.RangeDamage:
                    {
                        _skills.Add(new RangeDamageSkill(skill.Id, skill, this));
                        continue;
                    }
                    case SkillType.Self:
                    {
                        _skills.Add(new FullRegenerateManaSkill(skill.Id, skill, this));
                        continue;
                    }
                }
            }
        }

        public Inventory Inventory => _inventory;
        public List<ISkill> Skills => _skills;
        public float BasePhysicalDamage { get; set; }
        public float Speed { get; set; }
        public float Agility { get; set; }
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

        private async UniTask RegenerateMana()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                if (_currentMana + _regenerateManaPerSecond <= _maxMana)
                {
                    ChangeMana(_regenerateManaPerSecond);
                }
                
                await UniTask.Delay(1000);
            }
        }
        
        public void Destroy()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            foreach(var method in OnHealthEnded.GetInvocationList())
            {
                OnHealthEnded -= (Action)method;
            }
            
            foreach(var method in OnHealthChanged.GetInvocationList())
            {
                OnHealthChanged -= (Action<float>)method;
            }
            
            foreach(var method in OnNeedForNextLevelExperienceChanged.GetInvocationList())
            {
                OnNeedForNextLevelExperienceChanged -= (Action<float>)method;
            }
        }

        public int GetCost() => _cost;
    }
}