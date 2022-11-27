using System;
using System.Collections.Generic;
using System.Threading;
using Common.Enums;
using Cysharp.Threading.Tasks;
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

        public async UniTask CancelCurrentAnimationAsync(TimeSpan time, CancellationToken cancellationToken)
        {
            while (GetCurrentStateInfo().normalizedTime < 1)
            {
                try
                {
                    await UniTask.Delay(time, cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException _)
                {
                    return;
                }
            }
        }

        private AnimatorStateInfo GetCurrentStateInfo()
        {
            if (_animator is not null && _animator.gameObject.activeSelf)
                return _animator.GetCurrentAnimatorStateInfo(0);
            else return new AnimatorStateInfo();
        }
    }
}