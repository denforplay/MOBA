using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Models.Enemies;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Controllers.CombatControllers.AI.CreepCombatAI
{
    public abstract class AICombatControllerBase : BaseCombatController
    {
        protected TargetableView _target;
        protected readonly Creep _creep;
        protected readonly NavMeshAgent _navMeshAgent;
        private CancellationTokenSource _observingTokenSource;
        private CancellationTokenSource _attackingTokenSource;
        
        public AICombatControllerBase(CreepController creepController, Creep creep)
        {
            _navMeshAgent = creepController.Navigation;
            _creep = creep;
            _navMeshAgent.stoppingDistance = creep.AttackDistance;
        }

        private async UniTask ObserveAttackingDistance(TargetableView targetableView, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    
                    if (IsInAttackRange(_navMeshAgent.transform.position, targetableView.transform.position,
                            _creep.AttackDistance))
                    {
                        if (_canAttack)
                        {
                            Debug.Log("Start attack target");
                            if (_attackingTokenSource is null)
                                _attackingTokenSource = new CancellationTokenSource();

                            UniTask.Create(() => Attack(_attackingTokenSource.Token));
                        }
                    }
                    else
                    {
                        if (_attackingTokenSource is not null)
                        {
                            Debug.Log("Stop attacking target");
                            DeatachAttacking();
                            await UniTask.Delay(TimeSpan.FromMilliseconds(100), cancellationToken: cancellationToken);
                        }
                    }
                    
                    _navMeshAgent.SetDestination(_target.transform.position);
                    await UniTask.Delay(TimeSpan.FromMilliseconds(25), cancellationToken: cancellationToken);
                }
            }
            catch (Exception e)
            {
                return;
            }
            
        }

        public void Attack(TargetableView targetableView)
        {
            if (targetableView is null || _target is not null)
                return;

            Debug.Log("new target");
            _target = targetableView;
            _target.OnUntargeted += DeatachTarget;
            _observingTokenSource = new CancellationTokenSource();
            UniTask.Create(() => ObserveAttackingDistance(targetableView, _observingTokenSource.Token));
        }

        private void DeatachAttacking()
        {
            _attackingTokenSource.Cancel();
            _attackingTokenSource.Dispose();
            _attackingTokenSource = null;
        }

        private void DeatachObserving()
        {
            _observingTokenSource.Cancel();
            _observingTokenSource.Dispose();
            _observingTokenSource = null;
        }

        private void DeatachTarget()
        {
            _target.OnUntargeted -= DeatachTarget;
            _target = null;
            DeatachObserving();
            if (_attackingTokenSource is not null)
                DeatachAttacking();

            _canAttack = true;
        }
    }
}