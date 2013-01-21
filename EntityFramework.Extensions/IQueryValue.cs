namespace EntityFramework.Extensions
{
    public interface IQueryValue<out TValue, in TParameter>
    {
        TValue Get();
    }
}
