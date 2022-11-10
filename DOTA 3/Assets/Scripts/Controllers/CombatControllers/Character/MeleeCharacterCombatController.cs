using System;
using System.Threading;
using Common.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views;

namespace Controllers.CombatControllers.Character
{
    public class MeleeCharacterCombatController : CharacterCombatController
    {
        private TargetableView _previousTarget;
        private CancellationTokenSource _cancellationTokenSource;

        public MeleeCharacterCombatController(CharacterController characterController, Models.Character character) : base(characterController, character)
        {
        }
        
        public override void Attack(TargetableView target)
        {
            if (target == _previousTarget)
                return;
            
            DeatachTarget();
            
            if (target is null)
                return;

            target.Healthable.OnHealthEnded += DeatachTarget;
            _cancellationTokenSource = new CancellationTokenSource();
            target.SetAsTarget(true);
            target.OnUntargeted += () => _navigationAgent.stoppingDistance = 0;
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

        protected async UniTask Move(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
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

        private void DeatachTarget()
        {
            if (_previousTarget is not null)
            {
                _characterController.SetState(AnimationType.Walk);
                _previousTarget.Healthable.OnHealthEnded -= DeatachTarget;
                _cancellationTokenSource.Cancel();
                _previousTarget.SetAsTarget(false);
                _previousTarget = null;
            }
        }

        protected override async UniTask Attack(CancellationToken cancellationToken)
        {
            _canAttack = false;
            _navigationAgent.stoppingDistance = _character.AttackRange;
            
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (IsInAttackRange(_navigationAgent.transform.position, _previousTarget.transform.position,
                            _character.AttackRange))
                    {
                        _characterController.SetState(AnimationType.Attack);
                        await UniTask.Delay(TimeSpan.FromSeconds(_character.AttackDelay), cancellationToken: cancellationToken);
                        _previousTarget.ApplyDamage(_character.CurrentDamage);
                    }
                    else
                    {
                        await UniTask.Delay(TimeSpan.FromMilliseconds(20),  cancellationToken: cancellationToken);
                    }
                }
                catch (OperationCanceledException _)
                {
                    break;
                }
            }

            _navigationAgent.stoppingDistance = 0;
            _canAttack = true;
        }
    }
}