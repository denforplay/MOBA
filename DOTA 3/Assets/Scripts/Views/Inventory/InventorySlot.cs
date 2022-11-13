using Models.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        private bool _isFree = true;

        public bool IsFree => _isFree;

        public void SetSlotItem(Item item)
        {
            _itemImage.sprite = item.Sprite;
            _isFree = false;
        }

        public void FreeSlotItem()
        {
            _itemImage.sprite = null;
            _isFree = true;
        }
    }
}