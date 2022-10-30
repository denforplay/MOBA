using System;
using System.Linq;
using System.Threading;
using Common.Abstracts;
using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Controllers
{
    public class CreepController : IController
    {
        public event Action OnAttack;
        
        private readonly AnimationController _animationController;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Direction _direction;
        private WayPoint _currentWayPoint;
        private readonly CreepConfiguration _configuration;
        private CancellationTokenSource _cancellationObserveToken;
        private bool _isObserved = false;
        private bool _isWasObserved = false;
        
        public NavMeshAgent Navigation => _navMeshAgent;
        public AnimationController AnimationController => _animationController;

        public void SetState(AnimationType animationType)
        {
            _animationController.ChangeAnimation(animationType);
        }
        
        public CreepController(NavMeshAgent navMeshAgent, CreepConfiguration configuration, AnimationController animationController, WayPoint startWayPoint, Direction direction)
        {
            _animationController = animationController;
            _navMeshAgent = navMeshAgent;
            _currentWayPoint = startWayPoint;
            _direction = direction;
            _configuration = configuration;
        }
        
        public void StartMove()
        {
            _cancellationObserveToken = new CancellationTokenSource();
            UniTask.Create(Move);
            UniTask.Create(ObserveEnemies);
        }

        private async UniTask ObserveEnemies()
        {
            while (!_cancellationObserveToken.IsCancellationRequested)
            {
                var something = Physics.OverlapSphere(_navMeshAgent.transform.position, _configuration.ObservingRadius);
                var characterView =
                    something.FirstOrDefault(x => x.gameObject.TryGetComponent(out CharacterView _));
                
                if (characterView is not null)
                {
                    _navMeshAgent.SetDestination(characterView.transform.position);
                    
                    _isObserved = true;
                }
                else if (_isObserved)
                {
                    _isWasObserved = true;
                    _isObserved = false;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async UniTask Move()
        {
            while (_currentWayPoint is not null)
            {
                if (_navMeshAgent.remainingDistance <= 0.5 && !_isObserved)//todo: refactoring
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