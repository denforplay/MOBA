using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Character info", order = 0)]
    public class CharacterInfo : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private float _speed;

        private int Id => _id;
        public float Speed => _speed;
    }
}