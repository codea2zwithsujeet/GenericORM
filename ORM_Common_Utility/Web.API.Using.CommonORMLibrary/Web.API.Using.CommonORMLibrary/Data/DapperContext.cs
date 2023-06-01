using Microsoft.Extensions.Configuration;
using System.Data;

namespace Web.API.Using.CommonORMLibrary.Data
{
    public class DapperContext: IDbConnection
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("CodeA2ZDbConnection"); ;
        }

        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        public int ConnectionTimeout => throw new System.NotImplementedException();

        public string Database => throw new System.NotImplementedException();

        public ConnectionState State => throw new System.NotImplementedException();

        public IDbTransaction BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }
    }
}
