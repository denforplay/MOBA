using UnityEngine;

namespace Configurations.Levels
{
    [CreateAssetMenu(menuName = "Configurations/Levels/Level configuration", order = 0)]
    public class LevelConfiguration : ScriptableObject
    {
        [SerializeField] private float _experienceNeeded;
        [SerializeField] private float _upgradePoints;
        [SerializeField] private float _additionalStrength;
        [SerializeField] private float _additionalIntelligence;
        [SerializeField] private float _additionalAgility;

        public float ExperienceNeeded => _experienceNeeded;
        public float UpgradePoints => _upgradePoints;
        public float AdditionalStrength => _additionalStrength;
        public float AdditionalIntelligence => _additionalIntelligence;
        public float AdditionalAgility => _additionalAgility;
    }
}