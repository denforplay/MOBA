namespace Common.Abstracts
{
    public interface IFactory<out T>
    {
        T Create();
    }
}