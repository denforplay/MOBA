using System;
using System.Collections.Generic;
using Models.Items;
using UnityEngine;
using Views.UI.Buttons;

namespace Views.Inventory
{
    public class ShopInventoryView : MonoBehaviour
    {
        [SerializeField] private List<ShopInventorySlotButton> _shopSlotButtons;

        public List<ShopInventorySlotButton> ShopSlotButtons => _shopSlotButtons;

        public void AddItem(Item item, int slot)
        {
            try
            {
                _shopSlotButtons[slot].InventorySlot.SetSlotItem(item);
            }
            catch (Exception e)
            {
            }
        }

        public void RemoveItem(int slot)
        {
            _shopSlotButtons[slot].Button.onClick.RemoveAllListeners();;
            _shopSlotButtons[slot].InventorySlot.FreeSlotItem();
        }
    }
}