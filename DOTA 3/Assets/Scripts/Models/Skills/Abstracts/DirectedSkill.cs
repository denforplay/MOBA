using Common.Abstracts;
using Common.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Models.Skills.Abstracts
{
    public abstract class DirectedSkill : ISkill
    {
        protected float _effectValue;

        public DirectedSkill(int skillId, float effectValue)
        {
            Id = skillId;
            _effectValue = effectValue;
            CanBeUsed = true;
        }

        public bool CanBeUsed { get; set; }

        public abstract UniTask Apply(Vector3 direction);

        public int Id { get; }

        public abstract SkillType SkillType { get; }
    }
}