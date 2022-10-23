using System;
using Common.Enums;
using Controllers.AnimationStateControllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Controllers.CombatControllers
{
    public class MeleeCharacterCombatController : CharacterCombatController
    {
        private EnemyView _previousTarget;
        
        public MeleeCharacterCombatController(CharacterController characterController, CharacterInfo characterInfo) : base(characterController, characterInfo)
        {
        }
        
        public override void Attack(EnemyView target)
        {
            if (target is null)
            {
                if (_previousTarget is not null)
                    _previousTarget.SetAsTarget(false);
                
                return;
            }
            
            target.SetAsTarget(true);
            _previousTarget = target;
            
            if (!CheckAttackRange(target))
            {
                var relativePosition = target.transform.position - _navigationAgent.gameObject.transform.position;
                Quaternion rotationToLookAt = Quaternion.LookRotation(relativePosition);
                float rotationY = Mathf.SmoothDampAngle(_navigationAgent.gameObject.transform.eulerAngles.y,
                    rotationToLookAt.y,
                    ref _rotateVelocity,
                    _rotateAttackSpeed * (Time.deltaTime * 5)
                );

                _navigationAgent.gameObject.transform.eulerAngles = new Vector3(0, rotationY, 0);
                _navigationAgent.SetDestination(_navigationAgent.gameObject.transform.position);
                if (_canAttack)
                {
                    Attack();
                    Debug.Log("Melee attack");
                }
            }
        }

        private async UniTask Attack()
        {
            _canAttack = false;
            _characterController.SetState(AnimationType.Attack);
            while (_characterController.GetCurrentStateInfo().normalizedTime < 1)
            {
                Debug.Log(_characterController.GetCurrentStateInfo().normalizedTime);
                await UniTask.Delay(TimeSpan.FromMilliseconds(50));
            }
            _canAttack = true;
        }
    }
}