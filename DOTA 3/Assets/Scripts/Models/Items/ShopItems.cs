using System.Collections.Generic;
using System.Linq;
using Common.Enums;
using Configurations.Items;
using Cysharp.Threading.Tasks;

namespace Models.Items
{
    public class ShopItems
    {
        public List<ItemEdge> StartItemEdges { get; }

        public ShopItems(ShopItemsConfiguration shopItemsConfiguration)
        {
            StartItemEdges = new List<ItemEdge>();
            foreach (var edgeConfig in shopItemsConfiguration.StartItemConfiguration)
            {
                StartItemEdges.Add(new ItemEdge(edgeConfig));
            }
        }

        public List<ItemEdge> SearchBranchWithType(ItemType itemType)
        {
            return StartItemEdges.Where(x => x.CurrentItem.ItemType == itemType).ToList();
        }
    }
}