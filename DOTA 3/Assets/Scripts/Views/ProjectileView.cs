using Common.Enums;
using UnityEngine;

namespace Views
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ProjectileType _projectileType;

        public Rigidbody Rigidbody => _rigidbody;
        public ProjectileType ProjectileType => _projectileType;
    }
}