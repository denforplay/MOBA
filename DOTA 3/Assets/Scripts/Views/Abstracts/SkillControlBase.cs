using Common.Enums;
using UnityEngine;

namespace Views.Abstracts
{
    public abstract class SkillControlBase : MonoBehaviour
    {
        public abstract void UpdateSkillView(Vector3 newPosition);
        public abstract SkillType SkillType { get; }
    }
}