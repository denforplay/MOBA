using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;
using Views;

namespace Controllers.CombatControllers.AI
{
    public abstract class AICombatControllerBase : BaseCombatController
    {
        private CharacterView _targetCharacter;
        private readonly CreepConfiguration _creepConfiguration;
        protected readonly NavMeshAgent _navMeshAgent;
        private readonly float _attackRange;

        public AICombatControllerBase(CreepController creepController, CreepConfiguration creepConfiguration)
        {
            _attackRange = creepConfiguration.AttackDistance;
            _navMeshAgent = creepController.Navigation;
            _creepConfiguration = creepConfiguration;
        }

        public void Attack(CharacterView characterView)
        {
            if (_targetCharacter is null)
            {
                _targetCharacter = characterView;
            }
            
            if (IsInAttackRange(_navMeshAgent.transform.position, characterView.transform.position, _attackRange) &&
                _canAttack)
            {
                UniTask.Create(Attack);
            }
        }
    }
}