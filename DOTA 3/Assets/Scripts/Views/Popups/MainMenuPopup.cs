using System;
using Common.PopupSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Views.Popups
{
    public class MainMenuPopup : Popup
    {
        [SerializeField] private Button _startGameButton;
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
                _popupSystem.SpawnPopup<GamePopup>(1);
            });
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