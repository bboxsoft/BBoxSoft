using BBoxSoft.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace BBoxSoft.Infrastructure.Database.Connection
{
    /// <summary>
    /// Administra la conexión y las transacciones de la base de datos.
    /// </summary>
    public class DbContext : IAsyncDisposable
    {
        /// <summary>
        /// Fábrica utilizada para crear conexiones SQL Server.
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Conexión activa del contexto.
        /// </summary>
        private SqlConnection? _connection;

        /// <summary>
        /// Transacción activa del contexto.
        /// </summary>
        private DbTransaction? _transaction;

        /// <summary>
        /// Inicializa una nueva instancia del contexto de base de datos.
        /// </summary>
        /// <param name="connectionFactory">
        /// Fábrica utilizada para crear conexiones SQL Server.
        /// </param>
        public DbContext(IConnectionFactory connectionFactory) { _connectionFactory = connectionFactory; }

        /// <summary>
        /// Indica si existe una transacción activa.
        /// </summary>
        public bool HasTransaction => _transaction != null;

        /// <summary>
        /// Obtiene la conexión actual. Si no existe la crea y si está cerrada,
        /// la abre automáticamente.
        /// </summary>
        /// <returns>
        /// Conexión SQL Server abierta.
        /// </returns>
        public async Task<SqlConnection> GetConnectionAsync()
        {
            if (_connection == null) { _connection = _connectionFactory.Create(); }

            if (_connection.State != ConnectionState.Open) { await _connection.OpenAsync(); }

            return _connection;
        }

        /// <summary>
        /// Crea un comando configurado para ejecutar un procedimiento almacenado de SQL Server.
        /// </summary>
        /// <param name="procedureName">
        /// Nombre del procedimiento almacenado.
        /// </param>
        /// <returns>
        /// Comando listo para agregar parámetros y ejecutarse.
        /// </returns>
        public async Task<SqlCommand> CreateCommandAsync(string procedureName)
        {
            // Obtener la conexión
            SqlConnection connection = await GetConnectionAsync();

            // Crear el comando
            SqlCommand command = connection.CreateCommand();

            // Configurar el procedimiento almacenado
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;

            // Guardar nombre del procedimiento como referencia
            command.SetProcedureName(procedureName);

            // Asociar la transacción activa, si existe
            if (_transaction != null) { command.Transaction = (SqlTransaction)_transaction; }

            return command;
        }

        /// <summary>
        /// Inicia una transacción.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null) { throw new InvalidOperationException("Ya existe una transacción activa."); }

            SqlConnection connection = await GetConnectionAsync();

            _transaction = await connection.BeginTransactionAsync();
        }

        /// <summary>
        /// Confirma la transacción.
        /// </summary>
        public async Task CommitAsync()
        {
            if (_transaction == null) { throw new InvalidOperationException("No existe una transacción activa."); }

            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
                await CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Revierte la transacción.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_transaction == null) { throw new InvalidOperationException("No existe una transacción activa."); }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
                await CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Cierra la conexión actual.
        /// </summary>
        public async Task CloseConnectionAsync()
        {
            if (_connection == null) { return; }

            await _connection.DisposeAsync();
            _connection = null;
        }

        /// <summary>
        /// Libera los recursos del contexto de base de datos.
        /// Revierte cualquier transacción activa y libera la conexión.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                try
                {
                    await _transaction.RollbackAsync();
                }
                finally
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }

            if (_connection != null)
            {
                await _connection.DisposeAsync();
                _connection = null;
            }
        }


    }

}
