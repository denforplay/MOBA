using Common.Abstracts;
using Common.Enums;
using UnityEngine;

namespace Models.Skills
{
    public class RangeTargetZoneSkill : ISkill
    {
        public bool CanBeUsed { get; set; }

        public RangeTargetZoneSkill(int skillId, float effectValue)
        {
            Id = skillId;
        }
        
        public void Apply(Vector3 position)
        {
        }

        public int Id { get; }
        public SkillType SkillType { get => SkillType.RangeTargetZone; }
    }
}