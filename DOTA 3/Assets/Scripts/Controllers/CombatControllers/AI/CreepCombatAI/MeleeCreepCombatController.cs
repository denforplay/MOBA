using System;
using System.Threading;
using Common.Enums;
using Cysharp.Threading.Tasks;
using Models.Enemies;

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
                if (_target is not null)
                    _target.ApplyDamage(_creep.Damage);
            }
            finally
            {
                if (!_creepController.IsDestroyed)
                {
                    await _creepController.AnimationController.CancelCurrentAnimationAsync(TimeSpan.FromMilliseconds(10), cancellationToken);
                    _creepController.SetState(AnimationType.Walk);
                }

                _canAttack = true;
            }
        }
    }
}