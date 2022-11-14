using Common.Enums;
using UnityEngine;

namespace Configurations.Items
{
    [CreateAssetMenu(menuName = "Configurations/Item value configuration", order = 0)]
    public class CharacteristicValueConfiguration : ScriptableObject
    {
        [SerializeField] private ItemValueType _valueType;
        [SerializeField] private Characteristic _characteristic;
        [SerializeField] private string _valueDescription;
        [SerializeField] private float _value;

        public ItemValueType ItemValueType => _valueType;
        public Characteristic Characteristic => _characteristic;
        public string ValueDescription => _valueDescription;
        public float Value => _value;
    }
}