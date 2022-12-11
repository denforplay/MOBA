using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using Models.Skills.Abstracts;
using UnityEngine;

namespace Models.Skills.Skills
{
    public class FullRegenerateManaSkill : SelfAppliedSkill
    {
        public FullRegenerateManaSkill(int skillId, SkillConfiguration skillConfiguration, Character character) : base(skillId, skillConfiguration, character)
        {
        }
        
        public override async UniTask Apply(Vector3 position)
        {
            _character.ChangeMana(_character.MaxMana - _character.CurrentMana);
        }

        public override SkillType SkillType { get => SkillType.Self; }
    }
}