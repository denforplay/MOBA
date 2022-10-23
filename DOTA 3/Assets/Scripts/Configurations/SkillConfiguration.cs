using Common.Enums;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Skill configuration", order = 0)]
    public class SkillConfiguration : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Sprite _skillSprite;
        [SerializeField] private float _countdownTime;
        [SerializeField] private SkillType _skillType;
        [SerializeField] private AnimationClip _skillAnimation;
        
        public int Id => _id;
        public Sprite SkillSprite => _skillSprite;
        public float CountDowntime => _countdownTime;
        public SkillType SkillType => _skillType;
        public AnimationClip SkillAnimation => _skillAnimation;
    }
}