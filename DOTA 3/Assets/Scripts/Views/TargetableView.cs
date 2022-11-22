using System;
using System.Collections.Generic;
using Common.Abstracts;
using Common.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class TargetableView : MonoBehaviour
    {
        [SerializeField] private Image _targetMark;
        [SerializeField] private HealthView _healthView;
        private Dictionary<object, Action> _isTargetFor;

        private IHealthable _healthable;
        private ICostable _costable;
        
        private Team _team;

        public Team Team => _team;
        
        private void Awake()
        {
            _targetMark.enabled = false;
            _isTargetFor = new Dictionary<object, Action>();
        }

        public void Subscribe(object subscriber, Action callBack, bool isMarked = true)
        {
            if (!_isTargetFor.TryGetValue(subscriber, out var previousCallback))
            {
                if (isMarked)
                {
                    _targetMark.enabled = true;
                    _targetMark.gameObject.SetActive(true);
                }
                _isTargetFor.Add(subscriber, callBack);
            }
            else
            {
                _isTargetFor[subscriber] += callBack;
            }
        }

        public void UnSubscribe(object subscriber, bool isMarked)
        {
            if (_isTargetFor.TryGetValue(subscriber, out var callback))
            {
                callback?.Invoke();
            }
            
            _isTargetFor.Remove(subscriber);

            if (_isTargetFor.Count == 0 && isMarked)
            {
                try
                {
                    _targetMark.enabled = false;
                    _targetMark.gameObject.SetActive(false);
                }
                catch
                {
                    return;
                }
            }
        }
        
        public void UnSubscribeAll()
        {
            foreach (var action in _isTargetFor.Values)
                action?.Invoke();

            _isTargetFor.Clear();
        }

        public IHealthable Healthable => _healthable;
        public ICostable Costable => _costable;

        public void AttachCostableModel(ICostable costable)
        {
            _costable = costable;
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

        public int GetCost()
        {
            return _costable?.GetCost() ?? 0;
        }

        private void SetCurrentHealth(float value)
        {
            _healthView.SetHealth(value);
        }
        
        public void SetTeam(Team team)
        {
            _team = team;
        }
    }
}