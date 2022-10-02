using UnityEngine;
using UnityEngine.UI;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Skill configuration", order = 0)]
    public class SkillConfiguration : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Sprite _skillSprite;
        [SerializeField] private float _countdownTime;

        public int Id => _id;
        public Sprite SkillSprite => _skillSprite;
        public float CountDowntime => _countdownTime;
    }
}