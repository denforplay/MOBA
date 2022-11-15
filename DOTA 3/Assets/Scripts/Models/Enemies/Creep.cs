using System;
using Common.Abstracts;
using Configurations;

namespace Models.Enemies
{
    public class Creep : IHealthable, ICostable
    {
        public event Action<float> OnMaxHealthChanged;
        public event Action<float> OnHealthChanged;
        public event Action OnHealthEnded;
        
        private float _maxHealth;
        private float _currentHealth;
        private int _cost;
        
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public float AttackRange { get; set; }
        public float AttackDistance { get; set; }
        public float Damage { get; set; }
        public float AttackDelay { get; set; }

        public Creep(CreepConfiguration configuration)
        {
            _maxHealth = configuration.MaxHealth;
            _currentHealth = _maxHealth;
            AttackRange = configuration.ObservingRadius;
            AttackDistance = configuration.AttackDistance;
            Damage = configuration.Damage;
            AttackDelay = configuration.AttackDelay;
            _cost = configuration.Cost;
        }

        public void ChangeHealth(float value)
        {
            _currentHealth += value;
            OnHealthChanged?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
                OnHealthEnded?.Invoke();
        }

        public int GetCost()
        {
            return _cost;
        }
    }
}