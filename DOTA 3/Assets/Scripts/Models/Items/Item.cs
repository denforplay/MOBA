using System.Collections.Generic;
using Common.Enums;
using Configurations.Items;
using UnityEngine;

namespace Models.Items
{
    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Sprite { get; }
        public ItemType ItemType { get; }
        public int Cost { get; }
        public List<CharacteristicValueConfiguration> ValueConfigurations { get; }

        public Item(ItemConfiguration itemConfiguration)
        {
            Name = itemConfiguration.ItemName;
            Description = itemConfiguration.ItemDescription;
            Sprite = itemConfiguration.ItemSprite;
            Cost = itemConfiguration.Cost;
            ItemType = itemConfiguration.ItemType;
            ValueConfigurations = itemConfiguration.ItemValueConfigurations;
        }
    }
}