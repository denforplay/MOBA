using System;
using System.Linq;
using System.Threading;
using Common.Abstracts;
using Cysharp.Threading.Tasks;
using Models.Towers;
using UnityEngine;
using Views;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;
using Views.Towers;

namespace Controllers.CombatControllers.AI.TowersAI
{
    public class TowerCombatController : IController
    {
        public event Action<Vector3> OnAttack;
        private TowerView _towerView;
        private CancellationTokenSource _observeCancellationToken;
        private bool _canAttack;
        private readonly Tower _tower;
        private readonly TargetableView _currentTarget;

        public TowerCombatController(TowerView towerView, Tower tower, ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> projectileFactory, ProjectileFactoryRequirement requirement)
        {
            _tower = tower;
            _towerView = towerView;
        }

        public void StartObserve()
        {
            _canAttack = true;
            _observeCancellationToken = new CancellationTokenSource();
            UniTask.Create(Observe);
        }
        
        public async UniTask Observe()
        {
            while (!_observeCancellationToken.IsCancellationRequested && !_towerView.IsDestroyed)
            {
                var colliders = Physics.OverlapSphere(_towerView.transform.position, _tower.Range);
                var foundedTarget = colliders.Select(x => x.GetComponent<TargetableView>()).
                    Where(x => x is not null && x.Team != _towerView.Team).
                    OrderBy(t => (t.transform.position - _towerView.transform.position).sqrMagnitude).
                    FirstOrDefault();
                if (foundedTarget is not null)
                {
                    var direction = GetDirection(_towerView.TowerGun.position, foundedTarget.transform.position);
                    _towerView.RotateInDirection(new Vector3(direction.x, 0, direction.z));
                    if (_canAttack)
                    {
                        UniTask.Create(() => Attack(direction));
                    }
                }
                await UniTask.Delay(TimeSpan.FromMilliseconds(10));
            }
        }

        private async UniTask Attack(Vector3 direction)
        {
            _canAttack = false;
            OnAttack?.Invoke(direction);
            await UniTask.Delay(TimeSpan.FromSeconds(_tower.ShootingDelay));
            _canAttack = true;
        }
        
        private Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            var heading = to - from;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        public void Cencel()
        {
            if (_observeCancellationToken is not null && _observeCancellationToken.Token.IsCancellationRequested)
            {
                _observeCancellationToken.Cancel();
                _observeCancellationToken.Dispose();
            }
        }
    }
}