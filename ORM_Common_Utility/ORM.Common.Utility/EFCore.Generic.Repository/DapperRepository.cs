using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Generic.Repository
{
    public class DapperRepository<TDapperContext> : IDapperRepository<TDapperContext> where TDapperContext : IDbConnection
    {
        private readonly TDapperContext _dbContext;
        public DapperRepository(TDapperContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string query)
        {
            var conn = _dbContext.ConnectionString;
            IDbConnection dapper =  new SqlConnection(conn);
            return await dapper.QueryAsync<TEntity>(query);
        }
        public async Task<int> AddAsync<TEntity>(TEntity entity, string query)
        {
            using (var connection = new SqlConnection(_dbContext.ConnectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, entity);
                return result;
            }
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync<TEntity>(string query)
        {
            using (var connection = new SqlConnection(_dbContext.ConnectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<TEntity>(query);
                return result.ToList();
            }
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string query,int id)
        {
            using (var connection = new SqlConnection(_dbContext.ConnectionString))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<TEntity>(query, new { Id = id });
                return result;
            }
        }

        public async Task<int> UpdateAsync<TEntity>(TEntity entity, string query)
        {
            using (var connection = new SqlConnection(_dbContext.ConnectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, entity);
                return result;
            }
        }

        public async Task<int> DeleteAsync(string query,int id)
        {
            using (var connection = new SqlConnection(_dbContext.ConnectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }
    }
}
