using System.Collections.Generic;
using Configurations.Items;

namespace Models.Items
{
    public class ItemEdge
    {
        public Item CurrentItem { get; set; }
        public List<ItemEdge> NextItems { get; set; }

        public ItemEdge(ItemEdgeConfiguration itemEdgeConfiguration)
        {
            CurrentItem = new Item(itemEdgeConfiguration.CurrentItemConfiguration);
            NextItems = new List<ItemEdge>();
            if (itemEdgeConfiguration.NextItemEdgeConfigurations is not null)
            {
                foreach (var edgeConfig in itemEdgeConfiguration.NextItemEdgeConfigurations)
                {
                    NextItems.Add(new ItemEdge(edgeConfig));
                }
            }
        }
    }
}