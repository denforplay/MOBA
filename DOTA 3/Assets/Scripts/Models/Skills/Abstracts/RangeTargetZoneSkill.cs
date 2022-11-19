using Common.Abstracts;
using Common.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Models.Skills.Abstracts
{
    public abstract class RangeTargetZoneSkill : ISkill
    {
        protected float _effectValue;
        protected float _range;
        public bool CanBeUsed { get; set; }

        public RangeTargetZoneSkill(int skillId, float effectValue, float range)
        {
            _range = range;
            _effectValue = effectValue;
            Id = skillId;
            CanBeUsed = true;
        }

        public abstract UniTask Apply(Vector3 position);

        public int Id { get; }
        public abstract SkillType SkillType { get; }
    }
}