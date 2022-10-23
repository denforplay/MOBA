using Common.Abstracts;
using UnityEngine;
using UnityEngine.AI;
using Views;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Controllers.CombatControllers
{
    public abstract class CharacterCombatController : IController
    {
        protected bool _canAttack = true;
        protected readonly NavMeshAgent _navigationAgent;
        private EnemyBase _targetedEnemy;
        private float _attackRange;
        protected float _rotateAttackSpeed;
        protected float _rotateVelocity;
        protected CharacterController _characterController;
        
        public CharacterCombatController(CharacterController characterController, CharacterInfo characterInfo)
        {
            _attackRange = characterInfo.AttackRange;
            _characterController = characterController;
            _navigationAgent = _characterController.NavigantionAgent;
        }

        public abstract void Attack(EnemyView target);

        protected bool CheckAttackRange(EnemyView target)
        {
            var distance = Vector3.Distance(_navigationAgent.gameObject.transform.position, target.transform.position);
            Debug.Log("Attack distance " + distance);
            if (distance > _attackRange)
            {
                _navigationAgent.SetDestination(target.transform.position);
                return true;
            }

            return false;
        }
    }
}