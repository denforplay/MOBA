using Common.Abstracts;
using Cysharp.Threading.Tasks;
using Models;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class CharacterController : IController
    {
        private Camera _camera;
        private NavMeshAgent _navigation;
        private PlayerInputs _inputs;
        private readonly Character _controlledCharacter;
        private Vector3 _skillPosition;
        
        public CharacterController(Camera camera, NavMeshAgent navAgent, Character controlledCharacter)
        {
            _camera = camera;
            _navigation = navAgent;
            _inputs = new PlayerInputs();
            _controlledCharacter = controlledCharacter;
        }

        public void Move()
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                _navigation.speed = _controlledCharacter.Speed;
                _navigation.SetDestination(hit.point);
            }
        }

        public async UniTask ObserveFirstSkillCoordinates(Vect)
        {
            while (true)
            {
            }
        }
    }
}