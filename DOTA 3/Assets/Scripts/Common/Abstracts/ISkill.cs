using Common.Enums;
using UnityEngine;

namespace Common.Abstracts
{
    public interface ISkill
    {
        bool CanBeUsed { get; set; }
        void Apply(Vector3 position);
        int Id { get; }
        SkillType SkillType { get; }
    }
}