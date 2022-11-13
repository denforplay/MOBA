using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;
using Common.PopupSystem;
using Configurations.Items;
using Models;
using Models.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Inventory;
using Views.UI.Buttons;

namespace Views.Popups
{
    public class ShopPopup : Popup
    {
        private string _buyButtonTemplate = "Buy({0})";
        private string _sellButtonTemplate = "Sell({0})";
        [SerializeField] private GameObject _itemDisplayPanel;
        [SerializeField] private ShopItemButton _shopItemButtonTemplate;
        [SerializeField] private List<ShopNavigationButton> _shopNavigationButtons;
        [SerializeField] private TextMeshProUGUI _choosenItemNameText;
        [SerializeField] private TextMeshProUGUI _choosenItemDescriptionText;
        [SerializeField] private Button _closeButton;
        [SerializeField] private BuySellButton _buySellButton;
        [SerializeField] private TextMeshProUGUI _currentMoneyText;
        [SerializeField] private ShopInventoryView _inventoryView;
        private ShopItems _shopItems;
        private ShopItemsConfiguration _configuration;
        private List<ShopItemButton> _currentShopItemButtons;
        private Item _currentItem;
        private Character _character;

        public void Initialize(ShopItemsConfiguration shopItemsConfiguration, Character character)
        {
            _closeButton.onClick.AddListener(OnClosing);
            _buySellButton.gameObject.SetActive(false);
            _currentShopItemButtons = new List<ShopItemButton>();
            _shopItems = new ShopItems(shopItemsConfiguration);
            foreach (var navigationButton in _shopNavigationButtons)
            {
                navigationButton.NavigationButton.onClick.AddListener(() => DisplayItemsWithType(navigationButton.ItemType));
            }

            _character = character;

            for (int i = 0; i < _character.Inventory.Items.Length - 1; i++)
            {
                if (_character.Inventory.Items[i] is not null)
                {
                    _inventoryView.ShopSlotButtons[i].InventorySlot.SetSlotItem(_character.Inventory.Items[i]);
                    int newI = i;
                    _inventoryView.ShopSlotButtons[i].Button.onClick.AddListener(() => 
                        ConfigureSellButton(_character.Inventory.Items[newI], newI));
                }
            }

            _currentMoneyText.text = _character.Money.ToString();
            _character.Inventory.OnItemChanged += OnInventoryItemUpdated;
            _character.OnMoneyChanged += OnMoneyUpdate;

            Closing += _ =>
            {
                _character.Inventory.OnItemChanged -= OnInventoryItemUpdated;
                _character.OnMoneyChanged -= OnMoneyUpdate;
            };
        }
        
        private void OnMoneyUpdate(int money)
        {
            _currentMoneyText.text = money.ToString();
            for (int i = 0; i < _currentShopItemButtons.Count; i++)
            {
                if (_currentShopItemButtons[i].Item.Cost > _character.Money)
                {
                    _currentShopItemButtons[i].MakeUnavailable();
                    _currentShopItemButtons[i].ItemButton.onClick.RemoveAllListeners();
                }
                else
                {
                    int newI = i;
                    _currentShopItemButtons[i].MakeAvailable();
                    _currentShopItemButtons[i].ItemButton.onClick.AddListener(() => DisplayItemInfo(_currentShopItemButtons[newI].Item));
                }
            }

            if (_currentItem is not null && _currentItem.Cost > _character.Money)
            {
                _buySellButton.Button.gameObject.SetActive(false);
                _buySellButton.Button.onClick.RemoveAllListeners();
                _currentItem = null;
            }
        }
        
        private void OnInventoryItemUpdated(int slot, Item item)
        {
            if (item is not null)
            {
                _inventoryView.AddItem(item, slot);
                _inventoryView.ShopSlotButtons[slot].Button.onClick.AddListener(() => 
                    ConfigureSellButton(_character.Inventory.Items[slot], slot));
            }
            else
                _inventoryView.RemoveItem(slot);
        }

        private List<Item> GetItemsFromEdges(List<ItemEdge> itemEdges)
        {
            List<Item> items = new List<Item>();
            
            for (int i = 0; i < itemEdges.Count; i++)
            {
                items.Add(itemEdges[i].CurrentItem);
                if (itemEdges[i].NextItems is not null)
                    items.AddRange(GetItemsFromEdges(itemEdges[i].NextItems));
            }

            return items;
        }

        public void DisplayItemsWithType(ItemType itemType)
        {
            foreach (var shopButton in _currentShopItemButtons)
            {
                shopButton.ItemButton.onClick.RemoveAllListeners();
                Destroy(shopButton.gameObject);
            }
            
            _currentShopItemButtons.Clear();
            _buySellButton.gameObject.SetActive(false);
            _currentItem = null;
            
            var items = GetItemsFromEdges(_shopItems.SearchBranchWithType(itemType));

            foreach (var item in items)
            {
                var itemButton = Instantiate(_shopItemButtonTemplate, _itemDisplayPanel.transform);
                itemButton.Initialize(item);
                _currentShopItemButtons.Add(itemButton);

                if (item.Cost > _character.Money)
                {
                    itemButton.MakeUnavailable();
                }
                else
                {
                    itemButton.ItemButton.onClick.AddListener(() => DisplayItemInfo(item));
                }
            }
        }

        private void DisplayItemInfo(Item item)
        {
            _currentItem = item;
            _choosenItemDescriptionText.text = item.Description;
            _choosenItemNameText.text = item.Name;
            _buySellButton.gameObject.SetActive(true);
            _buySellButton.Button.onClick.RemoveAllListeners();
            _buySellButton.Button.onClick.AddListener(BuyCurrentItem);
            _buySellButton.SetText(String.Format(_buyButtonTemplate, item.Cost));
        }

        private void BuyCurrentItem()
        {
            _character.Inventory.AddItem(_currentItem);
            _character.Money -= _currentItem.Cost;
        }

        private void ConfigureSellButton(Item item, int slot)
        {
            _currentItem = null;
            _buySellButton.gameObject.SetActive(true);
            _buySellButton.Button.onClick.RemoveAllListeners();
            _buySellButton.Button.onClick.AddListener(() => SellItem(item, slot));
            _buySellButton.SetText(String.Format(_sellButtonTemplate, item.Cost));
        }

        private void SellItem(Item item, int slot)
        {
            _buySellButton.Button.onClick.RemoveAllListeners();
            _buySellButton.gameObject.SetActive(false);
            _character.Inventory.RemoveItem(slot);
            _character.Money += item.Cost;
        }
        
        public override void EnableInput()
        {

        }

        public override void DisableInput()
        {

        }
    }
}