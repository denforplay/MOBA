using Common.Abstracts;
using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Models.Skills.Abstracts
{
    public abstract class DirectedSkill : ISkill
    {
        protected readonly SkillConfiguration _skillConfiguration;

        public DirectedSkill(int skillId, SkillConfiguration skillConfiguration)
        {
            Id = skillId;
            _skillConfiguration = skillConfiguration;
            CanBeUsed = false;
        }

        public bool CanBeUsed { get; set; }

        public abstract UniTask Apply(Vector3 direction);

        public int Id { get; }

        public abstract SkillType SkillType { get; }
    }
}