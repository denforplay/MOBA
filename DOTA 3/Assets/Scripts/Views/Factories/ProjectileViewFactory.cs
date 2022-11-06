using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Views.Abstracts;
using Views.Abstracts.FactoryRequirements;

namespace Views.Factories
{
    public class ProjectileViewFactory : ViewFactoryBase<ProjectileView, ProjectileFactoryRequirement>
    {
        [SerializeField] private List<ProjectileView> _projectilePrefabs;
        [SerializeField] private Transform _projectilesContainer;

        public override ProjectileView Create(ProjectileFactoryRequirement requirement)
        {
            var projectilePrefab = _projectilePrefabs.FirstOrDefault(x => x.ProjectileType == requirement.ProjectileType);
            var newProjectile = Instantiate(projectilePrefab, _projectilesContainer);
            newProjectile.transform.localScale = projectilePrefab.transform.localScale;
            return newProjectile;
        }
    }
}