using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq;

namespace ComputerShop.Data.Context
{
    public static class DbContextMetadata
    {
        private static MetadataWorkspace FindMetadataWorkspace(IObjectContextAdapter context)
        {
            var objectContext = context.ObjectContext;
            return objectContext.MetadataWorkspace;
        }

        private static ObjectSet<T> FindObjectSet<T>(IObjectContextAdapter context)
            where T : class
        {
            var objectContext = context.ObjectContext;
            //this can throw an InvalidOperationException if it's not mapped
            var objectSet = objectContext.CreateObjectSet<T>();
            return objectSet;
        }

        private static IEnumerable<NavigationProperty> FindNavigationPropertyCollection<T>(
            IObjectContextAdapter context)
            where T : class
        {
            var objectSet = FindObjectSet<T>(context);
            var elementType = objectSet.EntitySet.ElementType;
            var navigationProperties = elementType.NavigationProperties;
            return navigationProperties;
        }

        /// <summary>
        /// Finds the names of the entities in a DbContext.
        /// </summary>
        public static IEnumerable<string> FindEntities(DbContext context)
        {
            var metadataWorkspace = FindMetadataWorkspace(context);
            var items = metadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            return items.Select(t => t.FullName);
        }

        /// <summary>
        /// Finds the underlying table names.
        /// </summary>
        public static IEnumerable<string> FindTableNames(DbContext context)
        {
            var metadataWorkspace = FindMetadataWorkspace(context);
            //we don't have to force a metadata load in Code First, apparently
            var items = metadataWorkspace.GetItems<EntityType>(DataSpace.SSpace);
            //namespace name is not significant (it's not schema name)
            return items.Select(t => t.Name);
        }

        /// <summary>
        /// Finds the primary key property names for an entity of specified type.
        /// </summary>
        public static IEnumerable<string> FindPrimaryKey<T>(DbContext context)
            where T : class
        {
            var objectSet = FindObjectSet<T>(context);
            var elementType = objectSet.EntitySet.ElementType;
            return elementType.KeyMembers.Select(p => p.Name);
        }

        /// <summary>
        /// Determines whether the specified entity is transient.
        /// </summary>
        public static bool IsTransient<T>(DbContext context, T entity)
            where T : class
        {
            var pk = FindPrimaryKey<T>(context).First();
            //look it up on the entity
            var propertyInfo = typeof(T).GetProperty(pk);
            var propertyType = propertyInfo.PropertyType;
            //what's the default value for the type?
            var transientValue = propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
            //is the pk the same as the default value (int == 0, string == null ...)
            return propertyInfo.GetValue(entity, null) == transientValue;
        }

        /// <summary>
        /// Finds the navigation properties (References and Collections)
        /// </summary>
        public static IEnumerable<string> FindNavigationProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);
            return navigationProperties.Select(p => p.Name);
        }

        /// <summary>
        /// Finds the navigation collection properties.
        /// </summary>
        public static IEnumerable<string> FindNavigationCollectionProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);

            return from navigationProperty in navigationProperties
                   where navigationProperty.ToEndMember.RelationshipMultiplicity ==
                         RelationshipMultiplicity.Many
                   select navigationProperty.Name;
        }

        /// <summary>
        /// Finds the navigation reference properties
        /// </summary>
        public static IEnumerable<string> FindNavigationReferenceProperties<T>(DbContext context)
            where T : class
        {
            var navigationProperties = FindNavigationPropertyCollection<T>(context);

            return from navigationProperty in navigationProperties
                   let end = navigationProperty.ToEndMember
                   where end.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne ||
                         end.RelationshipMultiplicity == RelationshipMultiplicity.One
                   select navigationProperty.Name;
        }

        public static IEnumerable<KeyValuePair<string, EdmMember>> FindEntityToTableMapping<TEntity>(DbContext context)
            where TEntity: class
        {
            var modelMetadata = GetEntityMetadata<TEntity>(context);

            var tableMetadata = GetTableMetadata<TEntity>(context);

            return from modelMember in modelMetadata.Members
                   where modelMetadata.Members.IndexOf(modelMember) < tableMetadata.Members.Count
                   select new KeyValuePair<string, EdmMember>
                             (
                             modelMember.Name,
                             tableMetadata.Members[modelMetadata.Members.IndexOf(modelMember)]
                             );
        }
        
        public static EntityType GetEntityMetadata<TEntity>(DbContext context)
        {
            var metadataWorkSpace = FindMetadataWorkspace(context);

            var entityType = typeof(TEntity);

            //return metadataWorkSpace.GetItem<EntityType>(entityType.Name, DataSpace.CSpace);
            return metadataWorkSpace.GetItem<EntityType>(context.GetType().Namespace + "." + entityType.Name,
                                                         DataSpace.CSpace);
        }

        public static EntityType GetTableMetadata<TEntity>(DbContext context)
        {
            var metadataWorkSpace = FindMetadataWorkspace(context);

            var entityType = typeof(TEntity);

            return metadataWorkSpace.GetItem<EntityType>("CodeFirstDatabaseSchema." + entityType.Name, DataSpace.SSpace);
        }

        public static string FindMappedTableName<TEntity>(DbContext context)
        {
            var tableMetadata = GetTableMetadata<TEntity>(context);

            return tableMetadata.Name;
        }
    }
}