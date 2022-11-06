using Common.Abstracts;
using UnityEngine.AI;
using Views;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Controllers.CombatControllers
{
    public abstract class CharacterCombatController : BaseCombatController
    {
        private EnemyBase _targetedEnemy;
        protected float _attackRange;
        protected CharacterController _characterController;
        protected readonly NavMeshAgent _navigationAgent;
        protected readonly CharacterInfo _characterInfo;

        public CharacterCombatController(CharacterController characterController, CharacterInfo characterInfo)
        {
            _attackRange = characterInfo.AttackRange;
            _characterController = characterController;
            _characterInfo = characterInfo;
            _navigationAgent = _characterController.NavigantionAgent;
        }

        public abstract void Attack(TargetableView target);
    }
}