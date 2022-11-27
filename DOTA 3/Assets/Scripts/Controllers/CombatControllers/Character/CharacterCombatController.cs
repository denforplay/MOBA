using System;
using System.Threading;
using Common.Enums;
using Controllers.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Controllers.CombatControllers.Character
{
    public abstract class CharacterCombatController : BaseCombatController
    {
        protected ICharacterController _characterController;
        protected readonly NavMeshAgent _navigationAgent;
        protected readonly Models.Character _character;
        protected TargetableView _previousTarget;
        protected CancellationTokenSource _cancellationTokenSource; 

        public CharacterCombatController(ICharacterController characterController, Models.Character character)
        {
            _character = character;
            _characterController = characterController;
            _navigationAgent = _characterController.NavigationAgent;
        }

        public virtual void Attack(TargetableView target)
        {
            if (target == _previousTarget && target is not null)
            {

                if (_canAttack && !(_cancellationTokenSource is null || _cancellationTokenSource.IsCancellationRequested))
                {
                    UniTask.Create(() => Attack(_cancellationTokenSource.Token));
                }
                
                return;
            }
            
            DeatachTarget();
            
            if (target is null)
                return;

            target.Healthable.OnHealthEnded += DeatachTarget;
            _cancellationTokenSource = new CancellationTokenSource();
            target.Subscribe(_character, OnUntargeted, true);
            _previousTarget = target;
            if (_canAttack)
            {
                UniTask.Create(() => Attack(_cancellationTokenSource.Token));
            }
            else
            {
                _navigationAgent.stoppingDistance = _character.AttackRange;
            }
            
            UniTask.Create(() => Move(_cancellationTokenSource.Token));
        }

        private void OnUntargeted()
        {
            _navigationAgent.stoppingDistance = 0;
        }
        
        protected async UniTask Move(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _characterController.IsDestroyed)
            {
                var distance = Vector3.Distance(_navigationAgent.gameObject.transform.position,
                    _previousTarget.gameObject.transform.position);


                if (distance > _character.AttackRange)
                {
                    _characterController.MoveTo(_previousTarget.transform.position);
                    _characterController.SetState(AnimationType.Walk);
                }
                else
                {
                    _navigationAgent.transform.LookAt(_previousTarget.transform);
                }

                await UniTask.Delay(TimeSpan.FromMilliseconds(10));
            }
        }

        protected void DeatachTarget()
        {
            if (_previousTarget is not null)
            {
                if (_previousTarget.Healthable.CurrentHealth <= 0)
                {
                    _character.Money += _previousTarget.GetCost();
                    _character.AddExperience(_previousTarget.GetCost());//TODO:REFACTORING
                }

                _characterController.SetState(AnimationType.Walk);
                _previousTarget.Healthable.OnHealthEnded -= DeatachTarget;
                _cancellationTokenSource.Cancel();
                _previousTarget.UnSubscribe(_character, true);
                _previousTarget = null;
            }
        }

        public void Cancel()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}