using System;
using Common.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI.Buttons
{
    public class ShopNavigationButton : MonoBehaviour
    {
        [SerializeField] private Button _navigationButton;
        [SerializeField] private ItemType _itemNavigationType;
        [SerializeField] private TextMeshProUGUI _buttonNavigationDescription;

        public Button NavigationButton => _navigationButton;
        public ItemType ItemType => _itemNavigationType;
        
        private void Awake()
        {
            _buttonNavigationDescription.text = _itemNavigationType.ToString();
        }
    }
}