using UnityEngine.AI;
using Views;

namespace Controllers.CombatControllers.Character
{
    public abstract class CharacterCombatController : BaseCombatController
    {
        protected CharacterController _characterController;
        protected readonly NavMeshAgent _navigationAgent;
        protected readonly Models.Character _character;

        public CharacterCombatController(CharacterController characterController, Models.Character character)
        {
            _character = character;
            _characterController = characterController;
            _navigationAgent = _characterController.NavigantionAgent;
        }

        public abstract void Attack(TargetableView target);
    }
}