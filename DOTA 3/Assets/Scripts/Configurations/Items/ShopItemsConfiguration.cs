using System.Collections.Generic;
using UnityEngine;

namespace Configurations.Items
{
    [CreateAssetMenu(menuName = "Configurations/Shop configuration", order = 0)]
    public class ShopItemsConfiguration : ScriptableObject
    {
        [SerializeField] private List<ItemEdgeConfiguration> _startItemEdgeConfigurations;

        public List<ItemEdgeConfiguration> StartItemConfiguration => _startItemEdgeConfigurations;
    }
}