using System;
using Common.Abstracts;
using Common.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class TargetableView : MonoBehaviour
    {
        public event Action OnUntargeted;
        
        [SerializeField] private Image _targetMark;
        [SerializeField] private HealthView _healthView;
        
        private IHealthable _healthable;
        
        private Team _team;

        public Team Team => _team;

        public IHealthable Healthable => _healthable;
        
        private void Awake()
        {
            _targetMark.enabled = false;
        }
        
        public void AttachHealthableModel(IHealthable healthable)
        {
            _healthable = healthable;
            _healthView.SetMaxHealth(_healthable.MaxHealth);
            _healthView.SetHealth(_healthable.CurrentHealth);
            _healthable.OnHealthChanged += SetCurrentHealth;
        }

        public void ApplyDamage(float value)
        {
            _healthable.ChangeHealth(-value);
        }
        

        private void SetCurrentHealth(float value)
        {
            _healthView.SetHealth(value);
        }
        
        public void SetTeam(Team team)
        {
            _team = team;
        }
        
        public void SetAsTarget(bool isTarget)
        {
            _targetMark.enabled = isTarget;
            _targetMark.gameObject.SetActive(isTarget);
            if (!isTarget)
                OnUntargeted?.Invoke();
        }
    }
}