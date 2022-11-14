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
            _navigationAgent.stoppingDistance = _character.AttackRange;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (IsInAttackRange(_navigationAgent.transform.position, _previousTarget.transform.position,
                            _character.AttackRange))
                    {
                        _characterController.SetState(AnimationType.Attack);
                        var projectile = _projectileFactory.CreateOnPosition(_navigationAgent.transform.position, _projectileFactoryRequirement);
                        projectile.Rigidbody.velocity =
                            GetDirection(_navigationAgent.transform.position, _previousTarget.transform.position).normalized * 20;
                        projectile.Setup(_character.CurrentDamage(), _character.Team);
                        await UniTask.Delay(TimeSpan.FromSeconds(_character.AttackDelay));
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

        private static Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            var heading = to - from;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }
    }
}