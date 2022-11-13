using System;
using Common.Exceptions;

namespace Models.Items
{
    public class Inventory
    {
        public event Action<int, Item> OnItemChanged;
        private Item[] _items;

        public Item[] Items => _items;
        
        public Inventory(int size)
        {
            _items = new Item[size];
        }

        public void AddItem(Item item, int slot = -1)
        {
            if (slot == -1 && (slot = FindFreeSlot()) == -1)
            {
                throw new NoFreeSlotsException();
            }

            _items[slot] = item;
            OnItemChanged?.Invoke(slot, item);
        }
        
        public void RemoveItem(int slot)
        {
            if (slot == -1 && (slot = FindFreeSlot()) == -1)
            {
                throw new NoFreeSlotsException();
            }

            _items[slot] = null;
            OnItemChanged?.Invoke(slot, null);
        }

        private int FindFreeSlot()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] is null)
                    return i;
            }

            return -1;
        }
    }
}