﻿using System;
using Common.Enums;
using Cysharp.Threading.Tasks;
using Views;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Controllers.CombatControllers.Character
{
    public class MeleeCharacterCombatController : CharacterCombatController
    {
        private TargetableView _previousTarget;

        public MeleeCharacterCombatController(CharacterController characterController, CharacterInfo characterInfo) : base(characterController, characterInfo)
        {
        }
        
        public override void Attack(TargetableView target)
        {
            if (target is null)
            {
                if (_previousTarget is not null)
                    _previousTarget.SetAsTarget(false);
                
                return;
            }
            
            target.SetAsTarget(true);
            _previousTarget = target;

            if (IsInAttackRange(_navigationAgent.transform.position, target.transform.position, _attackRange) && _canAttack)
            {
                UniTask.Create(Attack);
            }
        }

        protected override async UniTask Attack()
        {
            _canAttack = false;
            _characterController.SetState(AnimationType.Attack);

            while (_characterController.GetCurrentStateInfo().normalizedTime < 1)
            {
            }
            
            await UniTask.Delay(TimeSpan.FromMilliseconds(50));
            
            _canAttack = true;
        }
    }
}