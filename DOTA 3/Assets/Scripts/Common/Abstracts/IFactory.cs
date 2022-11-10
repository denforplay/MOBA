namespace Common.Abstracts
{
    public interface IFactoryRequirement
    {
        
    }
    
    public interface IFactory<out T, in TFactoryRequirement> where TFactoryRequirement : IFactoryRequirement
    {
        T Create(TFactoryRequirement requirement);
    }
}