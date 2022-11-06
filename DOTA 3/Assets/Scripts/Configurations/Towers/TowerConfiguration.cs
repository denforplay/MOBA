using UnityEngine;

namespace Configurations.Towers
{
    [CreateAssetMenu(menuName = "Configurations/Tower configuration", order = 0)]
    public class TowerConfiguration : ScriptableObject
    {
        [SerializeField] private float _health;
        [SerializeField] private float _damage;
        [SerializeField] private float _observableRange;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _shootDelay;
        
        public float Health => _health;
        public float Damage => _damage;
        public float ObservableRange => _observableRange;
        public float RotationSpeed => _rotationSpeed;
        public float ShootDelay => _shootDelay;
    }
}