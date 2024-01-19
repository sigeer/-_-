namespace DDDEF.Controllers
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
