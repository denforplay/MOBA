using Common.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Abstracts
{
    public interface ISkill
    {
        bool CanBeUsed { get; set; }
        UniTask Apply(Vector3 position);
        int Id { get; }
        SkillType SkillType { get; }
    }
}