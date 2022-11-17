using System;
using Common.Abstracts;
using Configurations.Towers;

namespace Models.Towers
{
    public class Tower : IHealthable
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
        
        public Tower(TowerConfiguration towerConfiguration)
        {
            _maxHealth = towerConfiguration.MaxHealth;
            _currentHealth = _maxHealth;
            Damage = towerConfiguration.Damage;
            Range = towerConfiguration.ObservableRange;
            ShootingDelay = towerConfiguration.ShootDelay;
        }
        
        public float Damage { get; set; }
        public float Range { get; set; }
        public float ShootingDelay { get; set; }
    }
}