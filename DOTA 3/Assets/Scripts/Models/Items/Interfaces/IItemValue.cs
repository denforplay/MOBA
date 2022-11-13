namespace Models.Items.Interfaces
{
    public interface IItemValue<out T>
    {
        T GetValue();
    }
}