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
        protected readonly SkillConfiguration _skillConfiguration;

        public SelfAppliedSkill(Character character, SkillConfiguration skillConfiguration)
        {
            _skillConfiguration = skillConfiguration;
            _character = character;
        }
        
        public bool CanBeUsed { get; set; }
        public abstract UniTask Apply(Vector3 position);

        public int Id { get; }
        public abstract SkillType SkillType { get; }
    }
}