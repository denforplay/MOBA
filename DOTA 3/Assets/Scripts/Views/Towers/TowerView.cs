using System;
using Common.Enums;
using Common.EventBus;
using Common.EventBus.Events;
using Configurations.Towers;
using Controllers.CombatControllers.AI.TowersAI;
using Models.Towers;
using UnityEngine;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;

namespace Views.Towers
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField] private TargetableView _targetableView;
        [SerializeField] private TowerConfiguration _configuration;
        [SerializeField] private GameObject _towerHead;
        [SerializeField] private Transform _towerGun;
        [SerializeField] private Team _team;
        [SerializeField] private ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> _projectileFactory;
        
        private Tower _tower;
        private TowerCombatController _combatController;

        public Transform TowerGun => _towerGun;
        public Transform TowerHead => _towerHead.transform;
        
        public Team Team => _team;


        private bool isDestroyed;
        public bool IsDestroyed => isDestroyed;
        private void Awake()
        {
            _tower = new Tower(_configuration);
            _targetableView.AttachHealthableModel(_tower);
            _targetableView.AttachCostableModel(_tower);
            _targetableView.SetTeam(_team);
            _tower.OnHealthEnded += Destroy;
            _combatController = new TowerCombatController(this, _tower, _projectileFactory, new ProjectileFactoryRequirement{ProjectileType = ProjectileType.CannonBall});
            _combatController.OnAttack += Attack;
            _combatController.StartObserve();
            EventBusManager.GetInstance.Subscribe<OnGameEndedEvent>(Destroy);
        }
        
        
        public void RotateInDirection(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                var step = _configuration.RotationSpeed * Time.deltaTime;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                Quaternion lookAtRotationOnlyY = Quaternion.Euler(_towerHead.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, _towerHead.transform.rotation.eulerAngles.z);
                _towerHead.transform.rotation = Quaternion.RotateTowards( _towerHead.transform.rotation, lookAtRotationOnlyY, step);
            }
        }

        public void SubscribeOnHealthEnded(Action action)
        {
            _tower.OnHealthEnded += action;
        }

        private void Destroy()
        {
            if (!isDestroyed)
            {
                isDestroyed = true;
                _combatController.Cencel();
                _combatController.OnAttack -= Attack;
                _tower.OnHealthEnded -= Destroy;
                Destroy(this.gameObject);
            }
        }
        
        private void Destroy(OnGameEndedEvent gameEndedEvent)
        {
            EventBusManager.GetInstance.Unsubscribe<OnGameEndedEvent>(Destroy);
            if (!isDestroyed)
            {
                isDestroyed = true;
                _combatController.Cencel();
                _combatController.OnAttack -= Attack;
                _tower.OnHealthEnded -= Destroy;
                Destroy(this.gameObject);
            }
        }

        private void Attack(Vector3 direction)
        {
            var projectile = _projectileFactory.CreateOnPosition(_towerGun.transform.position,
                new ProjectileFactoryRequirement { ProjectileType = ProjectileType.CannonBall });

            projectile.Setup(_tower.Damage, _team);
            projectile.Rigidbody.velocity = direction * 20;
        }

    }
}