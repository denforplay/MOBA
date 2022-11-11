using System;
using System.Linq;
using System.Threading;
using Common.Abstracts;
using Common.Enums;
using Cysharp.Threading.Tasks;
using Models.Enemies;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Controllers
{
    public class CreepController : IController
    {
        public event Action<TargetableView> OnAttack;
        
        private readonly AnimationController _animationController;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Direction _direction;
        private WayPoint _currentWayPoint;
        private CancellationTokenSource _cancellationObserveToken;
        private bool _isObserved = false;
        private bool _isWasObserved = false;
        private readonly CreepView _creepView;
        private readonly Creep _creep;

        public NavMeshAgent Navigation => _navMeshAgent;
        public AnimationController AnimationController => _animationController;

        public void SetState(AnimationType animationType)
        {
            _animationController.ChangeAnimation(animationType);
        }
        
        public CreepController(NavMeshAgent navMeshAgent, Creep creep, AnimationController animationController, WayPoint startWayPoint, Direction direction, CreepView creepView)
        {
            _creepView = creepView;
            _animationController = animationController;
            _navMeshAgent = navMeshAgent;
            _currentWayPoint = startWayPoint;
            _direction = direction;
            _creep = creep;
        }
        
        public void StartMove()
        {
            _cancellationObserveToken = new CancellationTokenSource();
            UniTask.Create(Move);
            UniTask.Create(ObserveEnemies);
        }

        private TargetableView _previousTarget;
        private async UniTask ObserveEnemies()
        {
            try
            {
                while (!_cancellationObserveToken.IsCancellationRequested)
                {
                    var foundedTargets = Physics.OverlapSphere(_navMeshAgent.transform.position, _creep.AttackRange);
                    var target = foundedTargets.
                        Select(x => x.GetComponent<TargetableView>()).
                        Where(x => x is not null && x.Team != _creepView.Team).
                        OrderBy(t => (t.transform.position - _creepView.transform.position).sqrMagnitude).
                        FirstOrDefault();
                

                    if (_previousTarget is null && target is not null)
                    {
                        _creepView.SubscribeOnDestroy(StopObserving);
                        OnAttack?.Invoke(target);
                        _previousTarget = target;
                        _navMeshAgent.SetDestination(target.transform.position);
                        _isObserved = true;
                    }
                    else if (target is null && _isObserved)
                    {
                        StopObserving();
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(0.1));
                }
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        private void StopObserving()
        {
            _creepView.UnSubscribeOnDestroy(StopObserving);
            _previousTarget.SetAsTarget(false);
            _previousTarget = null;
            _isWasObserved = true;
            _isObserved = false;
        }

        private async UniTask Move()
        {
            while (_currentWayPoint is not null)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 0.5 && !_isObserved)//todo: refactoring
                {
                    if (_isWasObserved)
                    {
                        _isWasObserved = false;
                        _navMeshAgent.SetDestination(_currentWayPoint.transform.position);
                        continue;
                    }
                    
                    if (_direction == Direction.Right)
                    {
                        _currentWayPoint = _currentWayPoint.NextRightWayPoint;
                    }
                    else if (_direction == Direction.Left)
                    {
                        _currentWayPoint = _currentWayPoint.NextLeftWayPoint;
                    }

                    _navMeshAgent.SetDestination(_currentWayPoint.transform.position);
                    _animationController.ChangeAnimation(AnimationType.Walk);
                }

                await UniTask.Delay(TimeSpan.FromMilliseconds(100));
            }
        }
    }
}