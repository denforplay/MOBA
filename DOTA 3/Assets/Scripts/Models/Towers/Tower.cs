using System;
using Common.Abstracts;
using Configurations.Towers;

namespace Models.Towers
{
    public class Tower : IHealthable
    {
        private float _currentHealth;
        private float _maxHealth;

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
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public event Action<float> OnHealthChanged;
        public event Action OnHealthEnded;
        public void ChangeHealth(float value)
        {
            throw new NotImplementedException();
        }
    }
}