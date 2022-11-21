using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Common.PopupSystem;
using Configurations.Items;
using UnityEngine;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;
using Zenject;
using CharacterInfo = Configurations.Character.CharacterInfo;

namespace Views.Popups
{
    public class GamePopup : Popup
    {
        [SerializeField] private ShopItemsConfiguration _shopItemsConfiguration;
        [SerializeField] private List<Transform> _startPositions;
        [SerializeField] private ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> _projectileViewFactory;
        
        private Camera _camera;
        private HudPopup _hudPopup;
        private PopupSystem _popupSystem;
        private CharacterView _controlledCharacterView;
        private CinemachineVirtualCamera _virtualCamera;

        [Inject]
        public void Inject(PopupSystem popupSystem, CinemachineVirtualCamera virtualCamera, Camera camera)
        {
            _popupSystem = popupSystem;
            _camera = camera;
            _virtualCamera = virtualCamera;
        }

        public void Initialize(CharacterInfo characterInfo)
        {
            var spawnPosition = _startPositions.First();
            var character = Instantiate(characterInfo.CharacterPrefab, transform);
            _controlledCharacterView = character.GetComponentInChildren<CharacterView>();
            _controlledCharacterView.Inject(_popupSystem, _projectileViewFactory);
            _hudPopup = _popupSystem.SpawnPopup<HudPopup>(0);
            _controlledCharacterView.Initialize(_camera, spawnPosition);
            var characterTransform = _controlledCharacterView.transform;
            _virtualCamera.LookAt = characterTransform;
            _virtualCamera.Follow = characterTransform;
            _hudPopup.Initialize(_controlledCharacterView);
            _hudPopup.OnShopCalled += ShowShopPopup;
        }

        private void ShowShopPopup()
        {
            var shopPopup = _popupSystem.SpawnPopup<ShopPopup>();
            shopPopup.Initialize(_shopItemsConfiguration, _controlledCharacterView.Character);
            shopPopup.Closing += _ => _controlledCharacterView.EnableInput();
        }
        
        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}