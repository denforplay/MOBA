using System;
using System.Collections.Generic;
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
        [SerializeField] private BattlegroundPanel _battlegroundPanel;
        [SerializeField] private TextMeshProUGUI _heroName;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private Button _shopButton;
        [SerializeField] private InventoryView _inventoryView;
        private CharacterView _characterView;

        public void Initialize(CharacterView characterView)
        {
            _characterView = characterView;
            _heroPanel.SetCharacterIcon(characterView.CharacterInfo.CharacterIcon);
            _heroPanel.SetSkills(characterView.CharacterInfo.SkillConfigurations);
            _heroPanel.SetCharacterStrength(characterView.CharacterInfo.BaseStrength);
            _heroPanel.SetCharacterDefense(characterView.CharacterInfo.BaseDefense);
            _heroPanel.SetCharacterIntelligence(characterView.CharacterInfo.BaseIntelligence);
            _heroPanel.SetCharacterLevel(characterView.Character.CurrentLevel);
            _heroName.text = characterView.CharacterInfo.HeroName;
            characterView.OnSkillActivated += _heroPanel.ResetSkill;
            characterView.SubscribeOnMoneyChanged((money) => _coinText.text = money.ToString());
            characterView.SubscribeOnInventoryChanged(UpdateInventoryView);
            foreach (var skillView in _heroPanel.SkillViews)
            {
                skillView.OnSkillUseStateChanged += characterView.SetSkillUseState;
            }
            
            SubscribeOnCharacterEvents();
            _shopButton.onClick.AddListener(OnShopPopupCalled);
        }

        public void InitializeBattleGroundPanel(List<Configurations.Character.CharacterInfo> leftTeamCharacters,
            List<Configurations.Character.CharacterInfo> rightTeamCharacters)
        {
            _battlegroundPanel.Initialize(leftTeamCharacters, rightTeamCharacters);
        }

        public void SubscribeOnCharacterEvents()
        {
            _characterView.Character.OnStrengthChanged += _heroPanel.SetCharacterStrength;
            _characterView.Character.OnDefenseChanged += _heroPanel.SetCharacterDefense;
            _characterView.Character.OnIntelligenceChanged += _heroPanel.SetCharacterIntelligence;
            _characterView.Character.OnLevelChanged += _heroPanel.SetCharacterLevel;
        }

        public void UnsubscribeFromCharacterEvents()
        {
            _characterView.Character.OnStrengthChanged -= _heroPanel.SetCharacterStrength;
            _characterView.Character.OnDefenseChanged -= _heroPanel.SetCharacterDefense;
            _characterView.Character.OnIntelligenceChanged -= _heroPanel.SetCharacterIntelligence;
            _characterView.Character.OnLevelChanged -= _heroPanel.SetCharacterLevel;
        }

        public void OnShopPopupCalled()
        {
            _characterView.DisableInput();
            OnShopCalled?.Invoke();
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