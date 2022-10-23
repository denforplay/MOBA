using Common.Abstracts;
using Common.Enums;
using UnityEngine;
using UnityEngine.UI;
using Views.Abstracts;

namespace Views.SkillControls
{
    public class DirectionSkillControl : SkillControlBase
    {
        [SerializeField] private Image _controlImage;
        
        public override void UpdateSkillView(Vector3 newPosition)
        {
            Quaternion rotation = Quaternion.LookRotation(newPosition - transform.position);
            rotation *= Quaternion.Euler(new Vector3(90, 0, 0));
            _controlImage.transform.rotation = rotation;
        }

        public override SkillType SkillType { get => SkillType.Directed; }
    }
}