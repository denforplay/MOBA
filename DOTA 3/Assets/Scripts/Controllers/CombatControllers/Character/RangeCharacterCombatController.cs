using System;
using System.Threading;
using Common.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;

namespace Controllers.CombatControllers.Character
{
    public class RangeCharacterCombatController : CharacterCombatController
    {
        private ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> _projectileFactory;
        private TargetableView _previousTarget;
        private ProjectileFactoryRequirement _projectileFactoryRequirement;

        public void Initialize(ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> projectileFactory, ProjectileFactoryRequirement requirement)
        {
            _projectileFactory = projectileFactory;
            _projectileFactoryRequirement = requirement;
        }
        
        public RangeCharacterCombatController(CharacterController characterController, Models.Character character) : base(characterController, character)
        {
        }
        
        protected override async UniTask Attack(CancellationToken cancellationToken)
        {
            _canAttack = false;
            _characterController.SetState(AnimationType.Attack);
            var projectile = _projectileFactory.CreateOnPosition(_navigationAgent.transform.position, _projectileFactoryRequirement);
            projectile.Rigidbody.velocity =
                GetDirection(_navigationAgent.transform.position, _previousTarget.transform.position).normalized * 20;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_character.AttackDelay));
            _canAttack = true;
        }

        private Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            var heading = to - from;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
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

            if (IsInAttackRange(_navigationAgent.transform.position, target.transform.position, _character.AttackRange) && _canAttack)
            {
                UniTask.Create(() => Attack(CancellationToken.None));
                Debug.Log("Melee attack");
            }
        }
    }
}