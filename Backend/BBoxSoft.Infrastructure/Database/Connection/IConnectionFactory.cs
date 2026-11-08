using Microsoft.Data.SqlClient;

namespace BBoxSoft.Infrastructure.Database.Connection
{
    /// <summary>
    /// Define el contrato para crear conexiones SQL Server
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Crea una nueva conexión SQL Server
        /// </summary>
        /// <returns>
        /// Instancia de <see cref="SqlConnection"/>.
        /// </returns>
        SqlConnection Create();
    }
}