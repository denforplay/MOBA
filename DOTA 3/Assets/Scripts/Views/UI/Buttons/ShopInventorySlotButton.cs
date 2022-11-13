using UnityEngine;
using UnityEngine.UI;
using Views.Inventory;

namespace Views.UI.Buttons
{
    public class ShopInventorySlotButton : MonoBehaviour
    {
        [SerializeField] private InventorySlot _inventorySlot;
        [SerializeField] private Button _button;

        public InventorySlot InventorySlot => _inventorySlot;
        public Button Button => _button;
    }
}