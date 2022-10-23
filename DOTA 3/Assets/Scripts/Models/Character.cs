using System.Collections.Generic;
using Common.Abstracts;
using Common.Enums;
using Configurations;
using Configurations.Character;
using Models.Skills;

namespace Models
{
    public class Character : IMovable
    {
        private List<ISkill> _skills;
        
        public Character(CharacterInfo characterInfo)
        {
            Speed = characterInfo.Speed;
            _skills = new List<ISkill>();
            foreach (var skill in characterInfo.SkillConfigurations)
            {
                switch (skill.SkillType)
                {
                    case SkillType.Directed:
                    {
                        _skills.Add(new DirectedSkill(skill.Id, 0));
                        continue;
                    }
                    case SkillType.RangeTargetZone:
                    {
                        _skills.Add(new RangeTargetZoneSkill(skill.Id, 0));
                        continue;
                    }
                }
            }
        }

        public List<ISkill> Skills => _skills;
        public float Speed { get; set; }
    }
}
