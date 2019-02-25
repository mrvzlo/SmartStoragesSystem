using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKitchen.Infrastructure.Repositories
{
    public static class RepositoryExtensions
    {
        public static void InsertOrUpdate<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            // Do not change state if entity is already attached
            if (!IsAttached(dbContext, entity))
            {
                var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
                var objectSet = objectContext.CreateObjectSet<TEntity>();

                var keyNames = objectSet.EntitySet.ElementType.KeyMembers.Select(x => x.Name);
                var hasDefaultKeys = keyNames.Any(keyName =>
                {
                    var property = typeof(TEntity).GetProperty(keyName);
                    var defaultValue = property.PropertyType.IsValueType
                        ? Activator.CreateInstance(property.PropertyType)
                        : null;
                    return Equals(property.GetValue(entity), defaultValue);
                });

                dbContext.Entry(entity).State =
                    hasDefaultKeys ? EntityState.Added : EntityState.Modified;
            }
            dbContext.SaveChanges();
        }

        public static bool IsAttached<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            return dbContext.Set<TEntity>().Local.Any(e => e == entity);
        }

        public static void Delete<TEntity>(this DbContext dbContext, TEntity entity) where TEntity : class
        {
            if (!IsAttached(dbContext, entity))
                dbContext.Entry(entity).State = EntityState.Deleted;

            dbContext.SaveChanges();
        }
    }
}
