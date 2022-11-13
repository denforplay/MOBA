using System.Collections.Generic;
using Common.Enums;
using UnityEngine;

namespace Configurations.Items
{
    [CreateAssetMenu(menuName = "Configurations/Item configuration", order = 0)]
    public class ItemConfiguration : ScriptableObject
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private string _itemName;
        [SerializeField] private int _cost;
        [SerializeField] private string _itemDescription;
        [SerializeField] private Sprite _itemSprite;
        [SerializeField] private List<ItemValueConfiguration> _itemValueConfigurations;

        public Sprite ItemSprite => _itemSprite;
        public ItemType ItemType => _itemType;
        public string ItemName => _itemName;
        public string ItemDescription => _itemDescription;
        public int Cost => _cost;
        public List<ItemValueConfiguration> ItemValueConfigurations => _itemValueConfigurations;
    }
}