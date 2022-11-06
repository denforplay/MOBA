using Common.Enums;
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
        [SerializeField] private TowerConfiguration _configuration;
        [SerializeField] private GameObject _towerHead;
        [SerializeField] private Transform _towerGun;
        [SerializeField] private Team _team;
        [SerializeField] private ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement> _projectileFactory;
        
        private Tower _tower;
        private TowerCombatController _combatController;

        public Team Team => _team;
        private void Awake()
        {
            _tower = new Tower()
            {
                Damage = _configuration.Damage,
                Range = _configuration.ObservableRange,
                Health = _configuration.Health,
                ShootingDelay = _configuration.ShootDelay,
            };

            _combatController = new TowerCombatController(this, _tower, _projectileFactory, new ProjectileFactoryRequirement{ProjectileType = ProjectileType.CannonBall});
            _combatController.OnAttack += Attack;
            _combatController.StartObserve();
        }

        private void Attack(Vector3 direction)
        {
            var projectile = _projectileFactory.CreateOnPosition(_towerGun.transform.position,
                new ProjectileFactoryRequirement { ProjectileType = ProjectileType.CannonBall });

            projectile.Rigidbody.velocity = direction * 20;
        }

        public void RotateInDirection(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                var step = _configuration.RotationSpeed * Time.deltaTime;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _towerHead.transform.rotation = Quaternion.RotateTowards( _towerHead.transform.rotation, lookRotation, step);
            }
        }
    }
}