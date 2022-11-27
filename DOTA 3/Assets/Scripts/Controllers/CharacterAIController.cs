using System;
using System.Linq;
using System.Threading;
using Common.Abstracts;
using Common.Enums;
using Controllers.Interfaces;
using Cysharp.Threading.Tasks;
using Models;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Controllers
{
    public class CharacterAIController : ICharacterController
    {
        public event Action<TargetableView> OnAttack;
        private readonly Camera _camera;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly AnimationController _animationController;
        private readonly Character _controlledCharacter;
        private WayPoint _currentWayPoint;
        private readonly Direction _direction;
        private bool _isObserved = false;
        private bool _isWasObserved = false;
        private CancellationTokenSource _cancellationObserveToken;
        private TargetableView _previousTarget;
        private readonly CharacterView _characterView;

        public NavMeshAgent NavigationAgent => _navMeshAgent;
        public bool IsDestroyed => _characterView.IsDestroyed;

        public CharacterAIController(Camera camera, NavMeshAgent navAgent, CharacterView characterView, Direction direction, WayPoint startWayPoint)
        {
            _characterView = characterView;
            _camera = camera;
            _navMeshAgent = navAgent;
            _animationController = characterView.AnimationController;
            _controlledCharacter = characterView.Character;
            _currentWayPoint = startWayPoint;
            _direction = direction;
            // _skillObservers = new Dictionary<SkillType, ISkillObserver>();
            // _skillObservers.Add(SkillType.Dash, new DirectedSkillObserver(_camera));
            // _skillObservers.Add(SkillType.RangeDamage, new RangeTargetZoneSkillObserver(_camera));
        }
        
        public void StartMove()
        {
            _cancellationObserveToken = new CancellationTokenSource();
            UniTask.Create(Move);
            UniTask.Create(ObserveEnemies);
        }


        private async UniTask Move()
        {
            try
            {
                while (_currentWayPoint is not null && !_cancellationObserveToken.Token.IsCancellationRequested)
                {
                    _navMeshAgent.speed = _controlledCharacter.Speed;
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
                            if (_currentWayPoint.NextRightWayPoint is not null)
                            {
                                _currentWayPoint = _currentWayPoint.NextRightWayPoint;
                            }
                        }
                        else if (_direction == Direction.Left)
                        {
                            if (_currentWayPoint.NextLeftWayPoint is not null)
                            {
                                _currentWayPoint = _currentWayPoint.NextLeftWayPoint;
                            }
                        }

                        _navMeshAgent.SetDestination(_currentWayPoint.transform.position);
                        _animationController.ChangeAnimation(AnimationType.Walk);
                    }

                    await UniTask.Delay(TimeSpan.FromMilliseconds(100));
                }
            }
            catch (Exception e)
            {
                Debug.Break();
            }
        }



        private async UniTask ObserveEnemies()
        {
            try
            {
                while (!_cancellationObserveToken.IsCancellationRequested)
                {
                    var foundedTargets = Physics.OverlapSphere(_navMeshAgent.transform.position, _controlledCharacter.AttackRange);
                    var target = foundedTargets.
                        Select(x => x.GetComponent<TargetableView>()).
                        Where(x => x is not null && x.Team != _controlledCharacter.Team).
                        OrderBy(t => (t.transform.position - _navMeshAgent.transform.position).sqrMagnitude).
                        FirstOrDefault();
                

                    if (target is not null)
                    {
                        if (_previousTarget is not null)
                            _characterView.UnSubscribeOnDestroy(StopObservingAll);
                        
                        _characterView.SubscribeOnDestroy(StopObservingAll);
                        OnAttack?.Invoke(target);
                        _previousTarget = target;
                        if (_navMeshAgent.enabled)
                            _navMeshAgent.SetDestination(target.transform.position);
                        _isObserved = true;
                    }
                    else if (_isObserved)
                    {
                        StopObserving();
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(0.1));
                }
            }
            catch (Exception e)
            {
                return;
            }
            
        }
        
        private void StopObserving()
        {
            _characterView.UnSubscribeOnDestroy(StopObserving);
            _previousTarget.UnSubscribe(_controlledCharacter, true);
            _previousTarget = null;
            _isWasObserved = true;
            _isObserved = false;
        }
        
        private void StopObservingAll()
        {
            if (_previousTarget is not null)
            {
                _characterView.UnSubscribeOnDestroy(StopObservingAll);
                _previousTarget.UnSubscribeAll();
                _previousTarget = null;
                _isWasObserved = true;
                _isObserved = false;
            }
        }

        void ICharacterController.Move()
        {
            
        }
        
        public void SetSkillUseState(int skillId, bool canBeUsed)
        {
        }

        
        public ISkillObserver ObserveSkill(int skillId, CancellationTokenSource cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        public void MoveTo(Vector3 point)
        {
            _navMeshAgent.ResetPath();
            point.y = _navMeshAgent.transform.position.y;
            _navMeshAgent.destination = point;
        }

        public void SetState(AnimationType animationType)
        {
            _animationController.ChangeAnimation(animationType);
        }

    }
}