using Bank.Credits.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Bank.Credits.Persistence.Extensions
{
    public static class DbSetExtensions
    {
        public static IQueryable<T> GetUndeleted<T>(this DbSet<T> dbSet) where T : SoftDeleteBaseEntity
        {
            return dbSet.Where(x => !x.DeleteDateTime.HasValue);
        }

        public static IQueryable<T1> GetUndeleted<T1, T2>(this IIncludableQueryable<T1, T2> dbSet) where T1 : SoftDeleteBaseEntity
        {
            return dbSet.Where(x => !x.DeleteDateTime.HasValue);
        }

        public static IQueryable<T1> GetUndeleted<T1, T2>(this IIncludableQueryable<T1, ICollection<T2>> dbSet) where T1 : SoftDeleteBaseEntity
        {
            return dbSet.Where(x => !x.DeleteDateTime.HasValue);
        }

        public static IQueryable<T> GetUndeleted<T>(this IQueryable<T> dbSet) where T : SoftDeleteBaseEntity
        {
            return dbSet.Where(x => !x.DeleteDateTime.HasValue);
        }

        public static IEnumerable<T> GetUndeleted<T>(this ICollection<T> collection) where T : SoftDeleteBaseEntity
        {
            return collection.Where(x => !x.DeleteDateTime.HasValue);
        }

        public static IEnumerable<T> GetDeleted<T>(this ICollection<T> collection) where T : SoftDeleteBaseEntity
        {
            return collection.Where(x => x.DeleteDateTime.HasValue);
        }
    }
}
