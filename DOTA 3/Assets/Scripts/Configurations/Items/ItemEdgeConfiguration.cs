using System.Collections.Generic;
using UnityEngine;

namespace Configurations.Items
{
    [CreateAssetMenu(menuName = "Configurations/Edge item configuration", order = 0)]
    public class ItemEdgeConfiguration : ScriptableObject
    {
        [SerializeField] private ItemConfiguration _currentItem;
        [SerializeField] private List<ItemEdgeConfiguration> _nextItemsEdgeConfigurations;

        public ItemConfiguration CurrentItemConfiguration => _currentItem;
        public List<ItemEdgeConfiguration> NextItemEdgeConfigurations => _nextItemsEdgeConfigurations;
    }
}