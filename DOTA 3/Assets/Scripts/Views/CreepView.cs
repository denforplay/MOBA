using System;
using System.Collections.Generic;
using Common.Enums;
using Configurations;
using Controllers;
using Controllers.CombatControllers.AI;
using UnityEngine;
using UnityEngine.AI;

namespace Views
{
    public class CreepView : MonoBehaviour
    {
        [SerializeField] private HealthView _healthView;
        [SerializeField] private CreepConfiguration _configuration;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        private WayPoint _startWayPoint;
        private CreepController _creepController;
        private AICombatControllerBase _combatController;
        private AnimationController _animationController;

        private Dictionary<CombatType, Func<CreepController, CreepConfiguration, AICombatControllerBase>> _combatControllersConfiguration =
            new Dictionary<CombatType, Func<CreepController, CreepConfiguration, AICombatControllerBase>>
            {
                { CombatType.Melee, (controller, config) => new CreepCombatController(controller, config) },
            };

        private AICombatControllerBase _creepCombatController;

        private void Awake()
        {
        }

        public void SetStartWayPoint(Direction direction, WayPoint wayPoint)
        {
            _startWayPoint = wayPoint;
            _animationController = new AnimationController(_configuration.AnimationsInfo, _animator);
            _creepController = new CreepController(_navMeshAgent, _configuration, _animationController, _startWayPoint, direction);
            _creepCombatController = _combatControllersConfiguration[_configuration.CombatType].Invoke(_creepController, _configuration);
            _creepController.StartMove();
        }
    }
}