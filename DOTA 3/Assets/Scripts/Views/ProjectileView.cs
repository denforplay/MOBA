using System;
using System.Threading;
using Assets.Scripts.Common.Abstracts;
using Common.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace Views
{
    public class ProjectileView : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ProjectileType _projectileType;
        [SerializeField] private float _lifeTime;
        private float _damage;
        private Team _team;
        private IMoneyable _projectileSource;

        public void Setup(float damage, Team team, IMoneyable projectileSource = null)
        {
            _projectileSource = projectileSource;
            _team = team;
            _damage = damage;
            _cancellationTokenSource = new CancellationTokenSource();
            UniTask.Create(() => BulletLife(_cancellationTokenSource.Token));
        }

        private async UniTask BulletLife(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime), cancellationToken: cancellationToken);
        }
        
        public Rigidbody Rigidbody => _rigidbody;
        public ProjectileType ProjectileType => _projectileType;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Projectile collide with " + other.gameObject);
            if (other.gameObject.TryGetComponent<TargetableView>(out var targetableView) &&
                targetableView.Team != _team)
            {
                targetableView.ApplyDamage(_damage);
                if (targetableView.Healthable.CurrentHealth <= 0)
                {
                    if (_projectileSource is not null)
                    {
                        _projectileSource.Money += targetableView.GetCost();
                    }
                }

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _cancellationTokenSource?.Cancel();
                    _cancellationTokenSource?.Dispose();
                }

                Destroy(this.gameObject);
            }
        }
    }
}