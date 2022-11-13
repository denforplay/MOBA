using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;
using Models.Items;
using UnityEngine;

namespace Views.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private List<InventorySlot> _inventorySlots;

        public bool IsHaveFreeSlot => _inventorySlots.Any(x => x.IsFree);

        public void RemoveItem(int slot)
        {
            _inventorySlots[slot].FreeSlotItem();
        }
        
        public void AddItem(Item item, int slot = -1)
        {
            InventorySlot inventorySlot = null;
            if (slot == -1)
            {
                inventorySlot = _inventorySlots.FirstOrDefault(x => x.IsFree);
            }
            else if (slot >= 0 && slot < _inventorySlots.Count && _inventorySlots[slot].IsFree)
            {
                inventorySlot = _inventorySlots[slot];
            }

            if (inventorySlot is null)
            {
                throw new NoFreeSlotsException();
            }
            
            inventorySlot.SetSlotItem(item);
        }
    }
}