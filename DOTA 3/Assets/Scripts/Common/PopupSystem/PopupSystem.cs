using System.Collections.Generic;
using System.Linq;
using Common.PopupSystem.Configurations;
using Models.Factories;
using UnityEngine;
using Views.Popups;
using Zenject;

namespace Common.PopupSystem
{
    public class PopupSystem : MonoBehaviour
    {
        [SerializeField] private List<Canvas> _canvases;
        [SerializeField] private Popup _startPopup;
        private readonly Stack<Popup> _popups = new Stack<Popup>();
        [SerializeField] private PopupSystemConfiguration _popupSystemConfig;
        private DiContainer _container;

        public void Initialize(DiContainer container)
        {
            _container = container;
            
        }

        private void Awake()
        {
            if (_startPopup)
            {
                Popup popupPrefab = _popupSystemConfig.PopupPrefabs.Find(a => a.GetType() == _startPopup.GetType());
                var factory = _container.Resolve<PopupFactory<MainMenuPopup>>();
                _popups.Push(factory.Create(popupPrefab, _canvases.First().transform));
            }
        }

        public T SpawnPopup<T>(int layer = 0) where T : Popup
        {
            Popup popupPrefab = _popupSystemConfig.PopupPrefabs.Find(a => a.GetType() == typeof(T));
            var canvas = _canvases[layer];
            Popup popup = _container.Resolve<PopupFactory<T>>().Create(popupPrefab, canvas.transform);
            popup.Closing += (popup) => DeletePopUp();
            _popups.Push(popup);
            popup.Show();
            return popup as T;
        }

        public void DeletePopUp()
        {
            Popup popup = _popups.Pop();
            popup.Hide();
        }

        public void DeleteAllPopups()
        {
            while (_popups.Count > 0)
            {
                var deletedPopup = _popups.Pop();
                deletedPopup.Hide();
            }
        }
    }
}