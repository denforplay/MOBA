using Common.Enums;
using UnityEngine;
using Views.Abstracts;

namespace Views.SkillControls
{
    public class SelfSkillControl : SkillControlBase
    {
        
        
        public override void UpdateSkillView(Vector3 newPosition)
        {
        }

        public override SkillType SkillType { get => SkillType.Self; }
    }
}