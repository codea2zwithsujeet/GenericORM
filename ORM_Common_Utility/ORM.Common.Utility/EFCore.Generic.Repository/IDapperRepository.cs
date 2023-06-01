using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EFCore.Generic.Repository
{
    public interface IDapperRepository<TDapperContext> where TDapperContext : IDbConnection
    {
        Task<TEntity> GetByIdAsync<TEntity>(string query,int id);
        Task<IReadOnlyList<TEntity>> GetAllAsync<TEntity>(string query);
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>( string query);
        Task<int> AddAsync<TEntity>(TEntity entity, string query);
        Task<int> UpdateAsync<TEntity>(TEntity entity, string query);
        Task<int> DeleteAsync(string query,int id);
    }
}
