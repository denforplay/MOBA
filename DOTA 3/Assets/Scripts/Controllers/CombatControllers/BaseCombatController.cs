using System.Threading;
using Common.Abstracts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Controllers.CombatControllers
{
    public abstract class BaseCombatController : IController
    {
        protected bool _canAttack = true;

        protected bool IsInAttackRange(Vector3 from, Vector3 to, float attackRange)
        {
            var distance = Vector3.Distance(from, to);
            if (distance > attackRange)
            {
                return false;
            }

            return true;
        }
        
        protected abstract UniTask Attack(CancellationToken cancellationToken);
    }
}