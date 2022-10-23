using System;
using Common.Enums;
using UnityEngine;
using UnityEngine.UI;
using Views.Abstracts;

namespace Views.SkillControls
{
    public class RangeTargetZoneSkillControl : SkillControlBase
    {
        [SerializeField] private Image _rangeCircle;
        [SerializeField] private float _maxDistance = 5f;
        [SerializeField] private CharacterView _player;

        public override void UpdateSkillView(Vector3 newPosition)
        {
            var hitDirection = (newPosition - transform.position).normalized;
            float distance = Vector3.Distance(newPosition, transform.position);
            distance = Mathf.Min(distance, _maxDistance);
            newPosition = _player.transform.position + hitDirection * distance;
            _rangeCircle.transform.position = newPosition;
        }

        public override SkillType SkillType { get => SkillType.RangeTargetZone; }
    }
}