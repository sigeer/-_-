using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDDomain.Shared.EntityProperty
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }

    public static class ISoftDeleteExtension
    {
        public static IQueryable<TEntity> ExceptDeleted<TEntity>(this IQueryable<TEntity> dataSource) where TEntity : ISoftDelete
        {
            return dataSource = dataSource.Where(x => !x.IsDeleted);
        }

        public static TEntity SoftDelete<TEntity>(this TEntity dbModel) where TEntity : ISoftDelete
        {
            dbModel.IsDeleted = true;
            return dbModel;
        }
    }
}
