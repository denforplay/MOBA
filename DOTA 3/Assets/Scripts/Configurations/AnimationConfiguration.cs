using System.Collections.Generic;
using Common.Enums;
using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(menuName = "Configurations/Animation configuration", order = 0)]
    public class AnimationConfiguration : ScriptableObject
    {
        [SerializeField] private AnimationClip _animationClip;
        [SerializeField] private AnimationType _animationType;

        public KeyValuePair<AnimationType, string> AnimationInfo =>
            new KeyValuePair<AnimationType, string>(_animationType, _animationClip.name);
    }
}