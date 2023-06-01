using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreUtility
{
    /// <summary>
    /// Contains all the repository methods. 
    /// If you register the multiple DbContexts, it will use the last one.
    /// To use specific <see cref="DbContext"/> please use <see cref="IRepository{TDbContext}"/>.
    /// </summary>
    public interface IRepository
    {
        Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class;
        Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
           where TEntity : class;
        void Update<TEntity>(TEntity entity)
          where TEntity : class;
        void Update<TEntity>(IEnumerable<TEntity> entities)
          where TEntity : class;
        void Remove<TEntity>(TEntity entity)
           where TEntity : class;
        void Remove<TEntity>(IEnumerable<TEntity> entities)
          where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<List<T>> GetFromRawSqlAsync<T>(string sql, CancellationToken cancellationToken = default);
        Task<List<T>> GetFromRawSqlAsync<T>(string sql, object parameter, CancellationToken cancellationToken = default);
        Task<List<T>> GetFromRawSqlAsync<T>(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsync<TEntity>(CancellationToken cancellationToken = default)
           where TEntity : class;

        Task<int> GetCountAsync<TEntity>(CancellationToken cancellationToken = default)
           where TEntity : class;
        Task<List<TEntity>> GetListAsync<TEntity>(CancellationToken cancellationToken = default)
            where TEntity : class;

        Task<TEntity> GetByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default)
           where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(object id, bool asNoTracking, CancellationToken cancellationToken = default)
           where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(
            object id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            CancellationToken cancellationToken = default)
            where TEntity : class;
    }
    /// <summary>
    /// Contains all the repository methods.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the <see cref="DbContext"/>.</typeparam>
    public interface IRepository<TDbContext> : IRepository
    {
    }
}
