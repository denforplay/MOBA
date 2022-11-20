using System;
using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using Models.Skills.Abstracts;
using UnityEngine;

namespace Models.Skills.Skills
{
    public class DashSkill : DirectedSkill
    {
        private readonly Character _character;

        public DashSkill(int skillId, SkillConfiguration skillConfiguration, Character character) : base(skillId, skillConfiguration)
        {
            _character = character;
        }
        
        public bool CanBeUsed { get; set; }

        public override async UniTask Apply(Vector3 position)
        {
            var direction = GetDirection(_character.NavMeshAgent.transform.position, position);
            _character.NavMeshAgent.ResetPath();
            await UniTask.Delay(TimeSpan.FromSeconds(0.05));
            _character.NavMeshAgent.speed = _character.Speed;
            var newVelocity = _skillConfiguration.EffectValue * direction;
            _character.NavMeshAgent.velocity = newVelocity;
        }
        
        private Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            var heading = to - from;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        public int Id { get; }
        public override SkillType SkillType { get => SkillType.Dash; }
    }
}