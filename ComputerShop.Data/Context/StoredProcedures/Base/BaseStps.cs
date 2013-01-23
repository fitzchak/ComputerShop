using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Context.StoredProcedures.Base
{
    public abstract class BaseStps
    {

    }

    public abstract class BaseStps<TResult, TEntity> 
        : BaseStps where TEntity : class
    {
        class UpdateStp : StoredProcedure<TResult>
        {
            protected TEntity Entity { get; set; }
            
            public UpdateStp(string name, StoredProcedureParameters parameters, string table, TableColumns columns, string keyName, string keyColumn, string keyType, TEntity entity) 
                : base(name, parameters, table, columns, keyName, keyColumn, keyType)
            {
                Entity = entity;
            }

            public override string GetCreateScript()
            {
                string template = @"
CREATE PROCEDURE {0} 
	{1}
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE {2} 
	    SET {3}
    WHERE
           ({4})
END
";
                var parametersChunk = string.Format("@{0} int\n\t,", KeyPropertyName);
                foreach (var parameter in Parameters)
                {
                    parametersChunk += string.Format("@{0} {1} = null\n\t,", parameter.Key, parameter.Value.Type);
                }
                parametersChunk = parametersChunk.Substring(0, parametersChunk.Length - 1);

                var columnsChunk = string.Empty;
                int parameterIndex = 0;
                foreach (var parameter in Parameters)
                {
                    columnsChunk += string.Format("{0} = @{1}\n\t\t,", Columns[parameterIndex], parameter.Key);
                    parameterIndex++;
                }
                columnsChunk = columnsChunk.Substring(0, columnsChunk.Length - 1);

                var whereChunk = string.Format("{0} = @{1}", KeyColumn, KeyPropertyName);

                var result = string.Format(template, Name, parametersChunk, Table, columnsChunk, whereChunk);

                return result;

            }

            public override void Execute(DbContext context)
            {
                var spCall = Name;

                var parameters = new List<SqlParameter>();

                var entityType = typeof (TEntity);

                var keyValue = entityType.GetProperty(KeyPropertyName).GetValue(Entity, new object[]{});

                parameters.Add(new SqlParameter(string.Format("@{0}", KeyPropertyName), keyValue));
                spCall += string.Format(" @{0},", KeyPropertyName);

                foreach (var parameter in Parameters)
                {
                    var propertyInfo = entityType.GetProperty(parameter.Key);
                    object value = null;

                    var propertyValue = propertyInfo.GetValue(Entity, new object[]{});
                    if (propertyValue == null)
                    {
                        value = DBNull.Value;
                    }
                    else if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof (IHaveId)))
                    {
                        value = ((IHaveId) propertyValue).Id;
                    }
                    else
                    {
                        value = propertyValue;
                    }

                    parameters.Add(new SqlParameter(string.Format("@{0}", parameter.Value.Name), value));

                    spCall += string.Format(" @{0},", parameter.Value.Name);
                }

                spCall = spCall.Substring(0, spCall.Length - 1);

                context.Database.SqlQuery<TResult>(spCall, parameters.Cast<object>().ToArray()).ToArray();
            }
        }

        class DeleteStp : StoredProcedure<TResult>
        {
            protected object KeyValue { get; set; }

            public DeleteStp(string name, StoredProcedureParameters parameters, string table, TableColumns columns, string keyName, string keyColumn, string keyType, object keyValue) : base(name, parameters, table, columns, keyName, keyColumn, keyType)
            {
                KeyValue = keyValue;
            }

            public override string GetCreateScript()
            {
                string template = @"
CREATE PROCEDURE {0} 
	{1}
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM {2} 
    WHERE({3})
    
END
";
                var parametersChunk = string.Format("@{0} {1}", KeyPropertyName, KeyType);
                var whereChunk = string.Format("{0} = @{1}", KeyColumn, KeyPropertyName);

                var result = string.Format(template, Name, parametersChunk, Table, whereChunk);

                return result;
            }

            public override void Execute(DbContext context)
            {
                var spCall = Name;

                var parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter(string.Format("@{0}", KeyPropertyName), KeyValue));

                spCall += string.Format(" @{0}", KeyPropertyName);

                context.Database.SqlQuery<TResult>(spCall, parameters.Cast<object>().ToArray()).ToArray();
            }
        }

        class InsertStp : StoredProcedure<TResult>
        {
            protected TEntity Entity { get; set; }

            public InsertStp(string name, StoredProcedureParameters parameters, string table, TableColumns columns, string keyName, string keyColumn, string keyType, TEntity entity) : base(name, parameters, table, columns, keyName, keyColumn, keyType)
            {
                Entity = entity;
            }

            public override string GetCreateScript()
            {
                string template = @"
CREATE PROCEDURE {0} 
	{1}
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO {2} 
		({3})
    VALUES
           ({4})
END
";
                var parametersChunk = string.Empty;
                foreach (var parameter in Parameters)
                {
                    parametersChunk += string.Format("@{0} {1} = null\n\t,", parameter.Key, parameter.Value.Type);
                }
                parametersChunk = parametersChunk.Substring(0, parametersChunk.Length - 1);

                var columnsChunk = string.Empty;
                foreach (var column in Columns)
                {
                    columnsChunk += string.Format("{0}\n\t\t,", column);
                }
                columnsChunk = columnsChunk.Substring(0, columnsChunk.Length - 1);

                var valuesChunk = string.Empty;
                foreach (var value in Parameters)
                {
                    valuesChunk += string.Format("@{0}\n\t\t,", value.Key);
                }
                valuesChunk = valuesChunk.Substring(0, valuesChunk.Length - 1);

                var result = string.Format(template, Name, parametersChunk, Table, columnsChunk, valuesChunk);

                return result;
            }

            public override void Execute(DbContext context)
            {
                var spCall = Name;

                var parameters = new List<SqlParameter>();

                var entityType = typeof(TEntity);

                foreach (var parameter in Parameters)
                {
                    var propertyInfo = entityType.GetProperty(parameter.Key);
                    object value = null;

                    var propertyValue = propertyInfo.GetValue(Entity, new object[]{});
                    if (propertyValue == null)
                    {
                        value = DBNull.Value;
                    }
                    else if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IHaveId)))
                    {
                        value = ((IHaveId)propertyValue).Id;
                    }
                    else
                    {
                        value = propertyValue;
                    }

                    parameters.Add(new SqlParameter(string.Format("@{0}", parameter.Value.Name), value));

                    spCall += string.Format(" @{0},", parameter.Value.Name);
                }
                spCall = spCall.Substring(0, spCall.Length - 1);

                context.Database.SqlQuery<TResult>(spCall, parameters.Cast<object>().ToArray()).ToArray();
            }
        }

        protected virtual StoredProcedureParameters Parameters { get; set; }
        protected virtual TableColumns Columns { get; set; }
        protected virtual string TableName { get; set; }
        protected virtual string KeyName { get; set; }
        protected virtual string KeyColumn { get; set; }
        protected virtual string KeyType { get; set; }

        protected BaseStps(DbContext context)
        {
            var tableMetadata = DbContextMetadata.GetTableMetadata<TEntity>(context);
            var entityMetadata = DbContextMetadata.GetEntityMetadata<TEntity>(context);

            TableName = tableMetadata.Name;

            var entityKeyMember = entityMetadata.KeyMembers.FirstOrDefault();
            var tableKeyMember = tableMetadata.KeyMembers.FirstOrDefault();

            KeyName = entityKeyMember.Name;
            KeyType = tableKeyMember.TypeUsage.EdmType.Name;
            KeyColumn = tableKeyMember.Name; // "CompID";

            Parameters = new StoredProcedureParameters();
            Columns = new TableColumns();

            foreach (var data in DbContextMetadata.FindEntityToTableMapping<TEntity>(context))
            {
                var columnMeta = data.Value;

                if (columnMeta.TypeUsage.Facets.Any(facet => facet.Name == "StoreGeneratedPattern"))
                {
                    continue;
                }

                Parameters.Add(data.Key, new StoredProcedureParameter(data.Key, columnMeta.TypeUsage.EdmType.Name));
                Columns.Add(columnMeta.Name);
            }
        }

        public virtual StoredProcedure<TResult> GetUpdateStp(TEntity entity)
        {
            return new UpdateStp(string.Format("stp_{0}_update", TableName), Parameters, TableName, Columns, KeyName, KeyColumn, KeyType, entity);
        }
        public virtual StoredProcedure<TResult> GetDeleteStp(object keyValue)
        {
            return new DeleteStp(string.Format("stp_{0}_delete", TableName), Parameters, TableName, Columns, KeyName, KeyColumn, KeyType, keyValue);
        }
        public virtual StoredProcedure<TResult> GetInsertStp(TEntity entity)
        {
            return new InsertStp(string.Format("stp_{0}_insert", TableName), Parameters, TableName, Columns, KeyName, KeyColumn, KeyType, entity);
        }
    }
}