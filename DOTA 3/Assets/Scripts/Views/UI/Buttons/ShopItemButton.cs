using System;
using Models.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.UI.Buttons
{
    public class ShopItemButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemPriceText;
        [SerializeField] private Image _fadeImage;
        [SerializeField] private Button _shopItemButton;
        private Item _item;

        public Item Item => _item;

        public void Initialize(Item item)
        {
            _item = item;
            _shopItemButton.image.sprite = item.Sprite;
            _itemNameText.text = item.Name;
            _itemPriceText.text = item.Cost.ToString();
        }

        public void MakeUnavailable()
        {
            _fadeImage.gameObject.SetActive(true);
        }

        public void MakeAvailable()
        {
            _fadeImage.gameObject.SetActive(false);
        }
        
        public Button ItemButton => _shopItemButton;
    }
}