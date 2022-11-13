using Common.Enums;
using UnityEngine;

namespace Configurations.Items
{
    [CreateAssetMenu(menuName = "Configurations/Item value configuration", order = 0)]
    public class ItemValueConfiguration : ScriptableObject
    {
        [SerializeField] private ItemValueType _valueType;
        [SerializeField] private string _itemDescription;

        public ItemValueType ValueType => _valueType;
        public string ItemDescription => _itemDescription;
    }
}