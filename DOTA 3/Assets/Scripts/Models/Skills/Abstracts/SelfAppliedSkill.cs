using Common.Abstracts;
using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Models.Skills.Abstracts
{
    public abstract class SelfAppliedSkill : ISkill
    {
        protected readonly Character _character;
        protected readonly int _skillId;
        protected readonly SkillConfiguration _skillConfiguration;

        public SelfAppliedSkill(int skillId, SkillConfiguration skillConfiguration, Character character)
        {
            _skillId = skillId;
            _skillConfiguration = skillConfiguration;
            _character = character;
            CanBeUsed = false;
        }
        
        public bool CanBeUsed { get; set; }
        public abstract UniTask Apply(Vector3 position);

        public int Id { get => _skillId; }
        public abstract SkillType SkillType { get; }
    }
}