using Cinemachine;
using Common.PopupSystem;
using Configurations.Items;
using UnityEngine;
using Zenject;

namespace Views.Popups
{
    public class GamePopup : Popup
    {
        [SerializeField] private ShopItemsConfiguration _shopItemsConfiguration;
        [SerializeField] private CharacterView _characterView;
        private Camera _camera;
        private HudPopup _hudPopup;
        private PopupSystem _popupSystem;

        [Inject]
        public void Initialize(PopupSystem popupSystem, CinemachineVirtualCamera virtualCamera, Camera camera)
        {
            _popupSystem = popupSystem;
            _camera = camera;
            _characterView.Initialize(camera);
            var characterTransform = _characterView.transform;
            virtualCamera.LookAt = characterTransform;
            virtualCamera.Follow = characterTransform;
            _hudPopup = popupSystem.SpawnPopup<HudPopup>(0);
            _hudPopup.Initialize(_characterView);
            _hudPopup.OnShopCalled += ShowShopPopup;
        }

        private void ShowShopPopup()
        {
            var shopPopup = _popupSystem.SpawnPopup<ShopPopup>();
            shopPopup.Initialize(_shopItemsConfiguration, _characterView.Character);
        }
        
        public override void EnableInput()
        {
        }

        public override void DisableInput()
        {
        }
    }
}