using Microsoft.Data.SqlClient;

namespace BBoxSoft.Infrastructure.Database.Connection
{

    /// <summary>
    /// Proporciona conexiones SQL Server
    /// </summary>
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) { throw new ArgumentNullException(nameof(connectionString)); }
            _connectionString = connectionString;
        }

        public SqlConnection Create()
        {
            return new SqlConnection(_connectionString);
        }

    }

}

