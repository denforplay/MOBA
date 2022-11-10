using System;
using System.Threading;
using Common.Enums;
using Cysharp.Threading.Tasks;
using Models.Enemies;
using UnityEngine;

namespace Controllers.CombatControllers.AI.CreepCombatAI
{
    public class CreepCombatController : AICombatControllerBase
    {
        private readonly CreepController _creepController;

        public CreepCombatController(CreepController creepController, Creep creep) : base(creepController, creep)
        {
            _creepController = creepController;
        }

        protected override async UniTask Attack(CancellationToken cancellationToken)
        {
            _canAttack = false;
            _creepController.SetState(AnimationType.Attack);
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_creep.AttackDelay), cancellationToken: cancellationToken);
                _target.ApplyDamage(_creep.Damage);
            }
            finally
            {
                await _creepController.AnimationController.CancelCurrentAnimationAsync(TimeSpan.FromMilliseconds(10));
                _creepController.SetState(AnimationType.Walk);
                _canAttack = true;
            }
        }
    }
}