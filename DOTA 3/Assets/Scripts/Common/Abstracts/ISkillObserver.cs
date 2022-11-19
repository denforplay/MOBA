using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Abstracts
{
    public interface ISkillObserver
    {
        event Action<Vector3> OnSkillCalled;
        event Action<Vector3> OnObservedPositionChanged;
        UniTask ObserveSkill(CancellationTokenSource cancellationToken);
    }
}