using System;
using Common.PopupSystem;
using Models.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Inventory;
using Views.UI.Panels;

namespace Views.Popups
{
    public class HudPopup : Popup
    {
        public event Action OnShopCalled;
        [SerializeField] private HeroPanel _heroPanel;
        [SerializeField] private Image _heroImage;
        [SerializeField] private TextMeshProUGUI _heroName;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private Button _shopButton;
        [SerializeField] private InventoryView _inventoryView;
        
        public void Initialize(CharacterView characterView)
        {
            _heroPanel.SetSkills(characterView.Skills);
            characterView.OnSkillActivated += _heroPanel.ResetSkill;
            characterView.SubscribeOnMoneyChanged((money) => _coinText.text = money.ToString());
            characterView.SubscribeOnInventoryChanged(UpdateInventoryView);
            foreach (var skillView in _heroPanel.SkillViews)
            {
                skillView.OnSkillUseStateChanged += characterView.SetSkillUseState;
            }
            
            _shopButton.onClick.AddListener(() => OnShopCalled?.Invoke());
        }

        private void UpdateInventoryView(int slot, Item item)
        {
            if (item is not null)
                _inventoryView.AddItem(item, slot);
            else
                _inventoryView.RemoveItem(slot);
        }
        
        public override void EnableInput()
        {
            enabled = true;
        }

        public override void DisableInput()
        {
            enabled = false;
        }
    }
}