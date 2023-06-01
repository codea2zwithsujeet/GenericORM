using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreUtility
{
    public class Repository<TDbContext> : IRepository,
        IRepository<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        }

        public Task<TEntity> GetAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return GetByIdAsync<TEntity>(id, false, cancellationToken);
        }
        public Task<T> GetByIdAsync<T>(object id, bool asNoTracking, CancellationToken cancellationToken = default)
           where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return GetByIdAsync<T>(id, null, asNoTracking, cancellationToken);
        }

        public Task<T> GetByIdAsync<T>(
           object id,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
           CancellationToken cancellationToken = default)
           where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return GetByIdAsync(id, includes, false, cancellationToken);
        }

        public Task<int> GetCountAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(string sql, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentNullException(nameof(sql));

            IEnumerable<object> parameters = new List<object>();

            List<T> items = await _dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);
            return items;
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(string sql, object parameter, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentNullException(nameof(sql));

            List<object> parameters = new List<object>() { parameter };
            List<T> items = await _dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);
            return items;
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentNullException(nameof(sql));

            List<T> items = await _dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);
            return items;
        }

        public Task<List<TEntity>> GetListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Remove<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<TEntity> trackedEntity = _dbContext.ChangeTracker
                .Entries<TEntity>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

                if (entityType == null)
                {
                    throw new InvalidOperationException($"{typeof(TEntity).Name} is not part of EF Core DbContext model");
                }

                string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

                if (primaryKeyName != null)
                {
                    Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

                    object primaryKeyDefaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

                    object primaryValue = entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null);

                    if (primaryKeyDefaultValue.Equals(primaryValue))
                    {
                        throw new InvalidOperationException("The primary key value of the entity to be updated is not valid.");
                    }
                }

                _dbContext.Set<TEntity>().Update(entity);
            }
        }

        public void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().UpdateRange(entities);
        }

        #region Common methods
        public async Task<T> GetByIdAsync<T>(
            object id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));

            string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            T enity = await query.FirstOrDefaultAsync(expressionTree, cancellationToken).ConfigureAwait(false);
            return enity;
        }
        #endregion
    }
}
