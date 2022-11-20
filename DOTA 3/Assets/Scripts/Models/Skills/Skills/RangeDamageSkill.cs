using Common.Enums;
using Configurations;
using Cysharp.Threading.Tasks;
using Models.Skills.Abstracts;
using UnityEngine;
using Views;

namespace Models.Skills.Skills
{
    public class RangeDamageSkill : RangeTargetZoneSkill
    {
        private readonly Character _character;

        public RangeDamageSkill(int skillId, SkillConfiguration skillConfiguration, Character character) : base(skillId, skillConfiguration)
        {
            _character = character;
        }

        public override async UniTask Apply(Vector3 position)
        {
            var hitDirection = (position - _character.NavMeshAgent.transform.position).normalized;
            float distance = Vector3.Distance(position, _character.NavMeshAgent.transform.position);
            distance = Mathf.Min(distance, _skillConfiguration.Range);
            position = _character.NavMeshAgent.transform.position + hitDirection * distance;
            
            var colliders = Physics.OverlapSphere(position, _skillConfiguration.Range);
            
            if (_skillConfiguration.ParticleSystem is not null)
            {
                var instantiatedParticles = GameObject.Instantiate(_skillConfiguration.ParticleSystem);
                instantiatedParticles.transform.position = position;
                //instantiatedParticles.main.duration = _skillConfiguration.
            }
            
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<TargetableView>(out var targetable) && targetable.Team != _character.Team)
                {
                    var resultDamage = _skillConfiguration.EffectValue * (_skillConfiguration.Range - Vector3.Distance(position, collider.transform.position)) / _skillConfiguration.Range;
                    targetable.ApplyDamage(resultDamage);
                }
            }
        }

        public override SkillType SkillType { get => SkillType.RangeDamage; }
    }
}