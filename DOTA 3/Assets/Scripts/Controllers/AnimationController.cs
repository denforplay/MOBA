using System.Collections.Generic;
using Common.Enums;
using UnityEngine;

namespace Controllers
{
    public class AnimationController
    {
        private Dictionary<AnimationType, string> _animations;
        private Animator _animator;
        private AnimationType _currentAnimation;
        
        public AnimationController(Dictionary<AnimationType, string> animations, Animator animator)
        {
            _animations = animations;
            _animator = animator;
        }

        public void ChangeAnimation(AnimationType animationType)
        {
            if (GetCurrentStateInfo().IsName(_animations[AnimationType.Attack]) && GetCurrentStateInfo().normalizedTime < 1)
            {
                return;
            }
            
            _animator.Play(_animations[animationType]);
        }

        public AnimatorStateInfo GetCurrentStateInfo()
        {
            return _animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}