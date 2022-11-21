using Common.Abstracts;
using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Models.Skills.Abstracts
{
    public abstract class RangeTargetZoneSkill : ISkill
    {
        protected readonly SkillConfiguration _skillConfiguration;
        public bool CanBeUsed { get; set; }

        public RangeTargetZoneSkill(int skillId, SkillConfiguration skillConfiguration)
        {
            _skillConfiguration = skillConfiguration;
            Id = skillId;
            CanBeUsed = false;
        }

        public abstract UniTask Apply(Vector3 position);

        public int Id { get; }
        public abstract SkillType SkillType { get; }
    }
}