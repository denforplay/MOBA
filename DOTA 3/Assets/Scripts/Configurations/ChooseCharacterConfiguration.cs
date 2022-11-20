using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Popups/Choose character config", order = 0)]
    public class ChooseCharacterConfiguration : ScriptableObject
    {
        [SerializeField] private float _secondsForChoose;
        [SerializeField] private float _secondsBeforeStart;

        public float SecondsForChoose => _secondsForChoose;
        public float SecondsBeforeStart => _secondsBeforeStart;
    }
}