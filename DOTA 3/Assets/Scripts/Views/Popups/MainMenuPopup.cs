using Common.PopupSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views.Popups
{
    public class MainMenuPopup : Popup
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _newsButton;
        
        private PopupSystem _popupSystem;

        [Inject]
        public void Initialize(PopupSystem popupSystem)
        {
            _popupSystem = popupSystem;
        }

        private void Awake()
        {
            _startGameButton.onClick.AddListener(() =>
            {
                _popupSystem.DeletePopUp();
                _popupSystem.SpawnPopup<ChooseCharacterPopup>();
            });

            _exitGameButton.onClick.AddListener(Application.Quit);
        }


        public override void EnableInput()
        {
            _startGameButton.interactable = true;
        }

        public override void DisableInput()
        {
            _startGameButton.interactable = false;
        }
    }
}