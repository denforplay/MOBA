using System;
using System.Threading;
using Common.Enums;
using Cysharp.Threading.Tasks;

namespace Controllers.CombatControllers.Character
{
    public class MeleeCharacterCombatController : CharacterCombatController
    {
        public MeleeCharacterCombatController(CharacterController characterController, Models.Character character) : base(characterController, character)
        {
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