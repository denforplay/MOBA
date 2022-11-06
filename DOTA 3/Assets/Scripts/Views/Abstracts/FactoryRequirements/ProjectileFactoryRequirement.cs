using Common.Abstracts;
using Common.Enums;

namespace Views.Abstracts.FactoryRequirements
{
    public class ProjectileFactoryRequirement : IFactoryRequirement
    {
        public ProjectileType ProjectileType { get; set; }
    }
}