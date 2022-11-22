using System.Threading;
using Common.Abstracts;
using Common.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers.Interfaces
{
    public interface ICharacterController : IController
    {
        ISkillObserver ObserveSkill(int skillId, CancellationTokenSource cancellationToken);
        void Move();
        void SetSkillUseState(int skillId, bool canBeUsed);
        void MoveTo(Vector3 position);
        NavMeshAgent NavigationAgent { get; }
        void SetState(AnimationType animationType);
    }
}