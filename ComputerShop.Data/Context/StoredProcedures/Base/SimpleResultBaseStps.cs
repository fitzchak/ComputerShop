using System.Data.Entity;

namespace ComputerShop.Data.Context.StoredProcedures.Base
{
    public abstract class SimpleResultBaseStps<TEntity> : BaseStps<SimpleStpResult, TEntity> 
        where TEntity : class
    {
        protected SimpleResultBaseStps(DbContext context) : base(context)
        {
        }
    }
}