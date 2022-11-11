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
            rotation.eulerAngles = new Vector3(90, rotation.eulerAngles.y, rotation.eulerAngles.z);
            _controlImage.transform.rotation = Quaternion.Lerp(rotation, _controlImage.transform.rotation, 0);
        }

        public override SkillType SkillType { get => SkillType.Directed; }
    }
}