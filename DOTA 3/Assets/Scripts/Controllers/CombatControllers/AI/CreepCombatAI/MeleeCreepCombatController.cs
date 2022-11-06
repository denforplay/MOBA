using System;
using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Controllers.CombatControllers.AI.CreepCombatAI
{
    public class CreepCombatController : AICombatControllerBase
    {
        private readonly CreepController _creepController;

        public CreepCombatController(CreepController creepController, CreepConfiguration creepConfiguration) : base(creepController, creepConfiguration)
        {
            _creepController = creepController;
        }

        protected override async UniTask Attack()
        {
            _canAttack = false;
            _creepController.SetState(AnimationType.Attack);
            while (_creepController.AnimationController.GetCurrentStateInfo().normalizedTime < 1)
            {
                Debug.Log(_creepController.AnimationController.GetCurrentStateInfo().normalizedTime);
                await UniTask.Delay(TimeSpan.FromMilliseconds(50));
            }

            _canAttack = true;
        }
    }
}