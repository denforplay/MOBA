using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using Models.Skills.Abstracts;
using UnityEngine;

namespace Models.Skills.Skills
{
    public class BoostParametersSkill : SelfAppliedSkill
    {
        
        public BoostParametersSkill(Character character, SkillConfiguration skillConfiguration) : base(character, skillConfiguration)
        {
        }
        
        public override async UniTask Apply(Vector3 position)
        {
        }

        public override SkillType SkillType { get; }

    }
}