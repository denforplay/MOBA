using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.AI;
using CharacterController = Controllers.CharacterController;

namespace Views
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Configurations.CharacterInfo _characterInfo;
        [SerializeField] private NavMeshAgent _navigationAgent;
        [SerializeField] private GameObject _characterGameObject;
        [SerializeField] private List<SkillView> _skills;
        private CharacterController _characterController;
        private Character _character;
        private Camera _camera;
        private PlayerInputs _playerInputs;

        public void Initialize(Camera camera)
        {
            _camera = camera;
            _character = new Character(_characterInfo);
            _characterController = new CharacterController(_camera, _navigationAgent, _character);
            _playerInputs = new PlayerInputs();
            _playerInputs.Character.Move.performed += _ => _characterController.Move();
            _playerInputs.Enable();
        }

        private void OnEnable()
        {
            _playerInputs.Enable();
        }

        private void OnDisable()
        {
            _playerInputs.Disable();
        }
    }
}