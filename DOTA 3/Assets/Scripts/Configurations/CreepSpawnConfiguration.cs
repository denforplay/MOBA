using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Creep spawn configuration", order = 0)]
    public class CreepSpawnConfiguration : ScriptableObject
    {
        [SerializeField] private int _packageSize;
        [SerializeField] private float _secondsBetweenPackages;
        [SerializeField] private float _secondsBetweenCreep;
        
        public int PackageSize => _packageSize;
        public float SecondsBetweenPackages => _secondsBetweenPackages;
        public float SecondsBetweenCreep => _secondsBetweenCreep;
    }
}