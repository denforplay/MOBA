using Common.EventBus;
using Common.EventBus.Events;
using Common.PopupSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views.Popups
{
    public class GameResultsPopup : Popup
    {
        private const string _winTextTemplate = "{0} team win!";
        
        [SerializeField] private TextMeshProUGUI _winText;
        [SerializeField] private Button _mainMenuButton;
        private PopupSystem _popupSystem;

        [Inject]
        public void Inject(PopupSystem popupSystem)
        {
            _popupSystem = popupSystem;
        }
        
        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(ShowMainMenu);
        }

        private void ShowMainMenu()
        {
            EventBusManager.GetInstance.Invoke(new OnGameEndedEvent());
            EventBusManager.GetInstance.Clear();
            _popupSystem.DeleteAllPopups();
            _popupSystem.SpawnPopup<MainMenuPopup>();
        }

        public override void EnableInput()
        {
            
        }

        public override void DisableInput()
        {
        }
    }
}