using System;
using System.Threading;
using Common.Abstracts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views;

namespace Models.Skills.Observers
{
    public class SelfSkillObserver : ISkillObserver
    {
        private readonly CharacterView _characterView;
        public event Action<Vector3> OnSkillCalled;
        public event Action<Vector3> OnObservedPositionChanged;

        public async UniTask ObserveSkill(CancellationTokenSource cancellationToken)
        {
            OnSkillCalled?.Invoke(Vector3.zero);
        }

        public void ClearSkillLCalled()
        {
        }
    }
}