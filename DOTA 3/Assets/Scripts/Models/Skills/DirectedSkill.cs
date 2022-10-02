using Common.Abstracts;
using UnityEngine;

namespace Models.Skills
{
    public class DirectedSkill : ISkill
    {
        private float _effectValue;

        public DirectedSkill(float effectValue)
        {
            _effectValue = effectValue;
        }
        
        public void Apply(Vector3 direction)
        {
            
        }
    }
}