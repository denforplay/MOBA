﻿using System;
using System.Collections.Generic;
using Common.Enums;
using Common.EventBus;
using Common.EventBus.Events;
using Configurations;
using Controllers;
using Controllers.CombatControllers.AI.CreepCombatAI;
using Models.Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace Views
{
    public class CreepView : MonoBehaviour
    {
        [SerializeField] private CreepConfiguration _configuration;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private TargetableView _targetableView;
        private WayPoint _startWayPoint;
        private CreepController _creepController;
        private AnimationController _animationController;
        private Creep _creep;

        private bool isDestroyed;
        public bool IsDestroyed => isDestroyed;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        private Dictionary<CombatType, Func<CreepController, Creep, AICombatControllerBase>> _combatControllersConfiguration =
            new Dictionary<CombatType, Func<CreepController, Creep, AICombatControllerBase>>
            {
                { CombatType.Melee, (controller, creep) => new CreepCombatController(controller, creep) },
            };

        public void SubscribeOnDestroy(Action action)
        {
            _targetableView.Healthable.OnHealthEnded += action;
        }
        
        public void UnSubscribeOnDestroy(Action action)
        {
            _targetableView.Healthable.OnHealthEnded -= action;
        }

        private AICombatControllerBase _creepCombatController;

        public Team Team => _targetableView.Team;

        private void Awake()
        {
            EventBusManager.GetInstance.Subscribe<OnGameEndedEvent>(DestroyOnGameEnded);
            _creep = new Creep(_configuration);
            _creep.OnHealthEnded += Destroy;
            _targetableView.AttachHealthableModel(_creep);
            _targetableView.AttachCostableModel(_creep);
        }

        private void Destroy()
        {
            if (!isDestroyed)
            {
                isDestroyed = true;
                EventBusManager.GetInstance.Unsubscribe<OnGameEndedEvent>(DestroyOnGameEnded);
                _creepCombatController.Cancel();
                _creepController.Cancel();
                Destroy(this.gameObject);
            }
        }

        private void DestroyOnGameEnded(OnGameEndedEvent e)
        {
            if (!isDestroyed)
            {
                Destroy();
            }
        }

        public void SetTeam(Team team)
        {
            _targetableView.SetTeam(team);
        }

        public void SetStartWayPoint(Direction direction, WayPoint wayPoint)
        {
            _startWayPoint = wayPoint;
            _animationController = new AnimationController(_configuration.AnimationsInfo, _animator);
            _creepController = new CreepController(_navMeshAgent, _creep, _animationController, _startWayPoint, direction, this);
            _creepCombatController = _combatControllersConfiguration[_configuration.CombatType].Invoke(_creepController, _creep);
            _creepController.OnAttack += _creepCombatController.Attack;
            _creepController.StartMove();
        }
    }
}