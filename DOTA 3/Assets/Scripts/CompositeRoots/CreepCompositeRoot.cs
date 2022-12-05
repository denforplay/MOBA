using System;
using System.Threading;
using Common.Abstracts;
using Common.Enums;
using Common.EventBus;
using Common.EventBus.Events;
using Configurations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views;
using Views.Abstracts.FactoryRequirements;
using Views.Factories;

namespace CompositeRoots
{
    public class CreepCompositeRoot : CompositeRoot
    {
        [SerializeField] private CreepSpawnConfiguration _creepSpawnConfiguration;
        [SerializeField] private CreepViewFactory _creepViewFactory;
        [SerializeField] private WayPointsWrapper _wayPointsWrapper;
        private CancellationTokenSource _cancellationToken;

        private void Awake()
        {
            Compose();
            EventBusManager.GetInstance.Subscribe<OnGameEndedEvent>(CancelOnGameEnded);
        }

        public override void Compose()
        {
            _cancellationToken = new CancellationTokenSource();
            UniTask.Create(() => StartCreepsSpawn(_cancellationToken.Token));
        }

        private async UniTask StartCreepsSpawn(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var packageSize = _creepSpawnConfiguration.PackageSize;
                    //UniTask.Create(() => CreateCreepsPackage(packageSize, GetStartWayPoint(_wayPointsWrapper.TopLineWayPoint, Direction.Left), Direction.Left, cancellationToken));
                    //UniTask.Create(() => CreateCreepsPackage(packageSize, GetStartWayPoint(_wayPointsWrapper.MidLineWayPoint, Direction.Left), Direction.Left, cancellationToken));
                    //UniTask.Create(() => CreateCreepsPackage(packageSize, GetStartWayPoint(_wayPointsWrapper.BottomLineWayPoint, Direction.Left), Direction.Left));
                    //UniTask.Create(() => CreateCreepsPackage(packageSize, GetStartWayPoint(_wayPointsWrapper.TopLineWayPoint, Direction.Right), Direction.Right));
                    //UniTask.Create(() => CreateCreepsPackage(packageSize, GetStartWayPoint(_wayPointsWrapper.MidLineWayPoint, Direction.Right), Direction.Right, cancellationToken));
                    //UniTask.Create(() => CreateCreepsPackage(packageSize, GetStartWayPoint(_wayPointsWrapper.BottomLineWayPoint, Direction.Right), Direction.Right));
                    await UniTask.Delay(TimeSpan.FromSeconds(_creepSpawnConfiguration.SecondsBetweenPackages), cancellationToken: cancellationToken);
                }
            }
            catch (OperationCanceledException _)
            {
                return;
            }
        }

        private async UniTask CreateCreepsPackage(int packageSize, WayPoint startWayPoint, Direction direction, CancellationToken cancellationToken)
        {
            for (int i = 0; i < packageSize; i++)
            {
                var newCreep = _creepViewFactory.CreateOnPosition(startWayPoint.transform.position,
                    new DefaultFactoryRequirement());
                ConfigureCreep(newCreep, direction, startWayPoint);
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_creepSpawnConfiguration.SecondsBetweenCreep), cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException _)
                {
                    return;
                }
            }
        }

        private WayPoint GetStartWayPoint(WayPoint wayPoint, Direction direction)
        {
            if (direction == Direction.Left)
            {
                while (wayPoint.NextRightWayPoint is not null)
                {
                    wayPoint = wayPoint.NextRightWayPoint;
                }
            }
            else if (direction == Direction.Right)
            {
                while (wayPoint.NextLeftWayPoint is not null)
                {
                    wayPoint = wayPoint.NextLeftWayPoint;
                }
            }

            return wayPoint;
        }
        
        private void ConfigureCreep(CreepView creepView, Direction direction, WayPoint wayPoint)
        {
            creepView.SetStartWayPoint(direction, wayPoint);
            
            if (direction == Direction.Left)
                creepView.SetTeam(Team.Red);
            else if (direction == Direction.Right)
                creepView.SetTeam(Team.Blue);
        }

        private void CancelOnGameEnded(OnGameEndedEvent e)
        {
            EventBusManager.GetInstance.Unsubscribe<OnGameEndedEvent>(CancelOnGameEnded);
            _cancellationToken.Cancel();
        }
    }
}