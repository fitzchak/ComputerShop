using System.Data.Entity;

namespace EntityFramework.Extensions
{
    abstract class QueryValue<TValue, TParameter> : IQueryValue<TValue, TParameter>
    {
        protected DbContext DbContext;

        protected QueryValue(DbContext dbContext)
        {
            DbContext =  dbContext;
        }

        public abstract TValue Get();
    }
}