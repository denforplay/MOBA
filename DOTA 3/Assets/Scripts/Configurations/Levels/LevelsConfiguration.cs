using System.Collections.Generic;
using UnityEngine;

namespace Configurations.Levels
{
    [CreateAssetMenu(menuName = "Configurations/Levels/Levels configuration", order = 0)]
    public class LevelsConfiguration : ScriptableObject
    {
        [SerializeField] private List<LevelConfiguration> _levelsConfigurations;

        public List<LevelConfiguration> LevelsConfigurations => _levelsConfigurations;
    }
}