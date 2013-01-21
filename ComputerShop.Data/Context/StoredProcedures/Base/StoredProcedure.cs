using System.Data.Entity;
using EntityFramework.Extensions;

namespace ComputerShop.Data.Context.StoredProcedures.Base
{
    public abstract class StoredProcedure<TResult> : IStoredProcedure<TResult>
    {
        protected virtual StoredProcedureParameters Parameters { get; set; }
        protected virtual TableColumns Columns { get; set; }
        protected virtual string Table { get; set; }
        protected virtual string Name { get; set; }
        protected virtual string KeyPropertyName { get; set; }
        protected virtual string KeyColumn { get; set; }
        protected virtual string KeyType { get; set; }

        protected StoredProcedure(string name, StoredProcedureParameters parameters, string table, TableColumns columns, string keyName, string keyColumn, string keyType)
        {
            Name = name;
            Parameters = parameters;
            Table = table;
            Columns = columns;
            KeyPropertyName = keyName;
            KeyColumn = keyColumn;
            KeyType = keyType;
        }

        public abstract string GetCreateScript();

        public abstract void Execute(DbContext context);
    }
}