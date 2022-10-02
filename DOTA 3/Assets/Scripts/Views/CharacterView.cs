using System.Collections.Generic;
using Configurations;
using Models;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using CharacterController = Controllers.CharacterController;

namespace Views
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Configurations.CharacterInfo _characterInfo;
        [SerializeField] private NavMeshAgent _navigationAgent;
        [SerializeField] private GameObject _characterGameObject;
        [SerializeField] private List<SkillConfiguration> _skills;
        [Header("Ability 1")] 
        private Image _directionImage;
        
        [Header("Ability 2")]
        
        private CharacterController _characterController;
        private Character _character;
        private Camera _camera;
        private PlayerInputs _playerInputs;

        public List<SkillConfiguration> Skills => _skills;

        public void Initialize(Camera camera)
        {
            _camera = camera;
            _character = new Character(_characterInfo);
            _characterController = new CharacterController(_camera, _navigationAgent, _character);
            _playerInputs = new PlayerInputs();
            _playerInputs.Character.Move.performed += _ => _characterController.Move();
            
            _playerInputs.Character.UseFirstSkill.started += _ => { };
            _playerInputs.Character.UseFirstSkill.canceled += _ => { };
            
            _playerInputs.Character.UseSecondSkill.started += _ => { };
            _playerInputs.Character.UseSecondSkill.canceled += _ => { };
            
            _playerInputs.Character.UseThirdSkill.started += _ => { };
            _playerInputs.Character.UseThirdSkill.canceled += _ => { };
            
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