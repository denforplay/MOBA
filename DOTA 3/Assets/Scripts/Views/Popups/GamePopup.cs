using Cinemachine;
using Common.PopupSystem;
using Controllers;
using UnityEngine;
using Zenject;
using CharacterController = UnityEngine.CharacterController;

namespace Views.Popups
{
    public class GamePopup : Popup
    {
        [SerializeField] private CharacterView _characterView;
        private Camera _camera;
        
        [Inject]
        public void Initialize(CinemachineVirtualCamera virtualCamera, Camera camera)
        {
            _camera = camera;
            _characterView.Initialize(camera);
            var characterTransform = _characterView.transform;
            virtualCamera.LookAt = characterTransform;
            virtualCamera.Follow = characterTransform;
        }
        
        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}