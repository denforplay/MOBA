using Common.Abstracts;
using Common.Enums;
using UnityEngine;

namespace Models.Skills
{
    public class DirectedSkill : ISkill
    {
        private float _effectValue;

        public DirectedSkill(int skillId, float effectValue)
        {
            Id = skillId;
            _effectValue = effectValue;
            CanBeUsed = false;
        }

        public bool CanBeUsed { get; set; }

        public void Apply(Vector3 direction)
        {
            
        }

        public int Id { get; }

        public SkillType SkillType { get => SkillType.Directed; }
    }
}