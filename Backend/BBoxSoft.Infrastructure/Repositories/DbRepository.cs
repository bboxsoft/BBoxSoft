using BBoxSoft.Infrastructure.Database.Connection;
using BBoxSoft.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace BBoxSoft.Infrastructure.Repositories
{

    /// <summary>
    /// Proporciona la funcionalidad base para ejecutar comandos de base de datos y mapear el resultado a entidades del dominio.
    ///
    /// Esta clase es la base de todos los DataAccess del sistema y encapsula la lógica común de ejecución y mapeo de resultados.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Tipo de entidad que será mapeada.
    /// </typeparam>
    public abstract class DbRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Contexto de base de datos utilizado para crear comandos y administrar la conexión y las transacciones.
        /// </summary>
        protected readonly DbContext _context;

        /// <summary>
        /// Cache de propiedades de la entidad para evitar reflexión en cada registro leído.
        /// </summary>
        private static readonly PropertyInfo[] _properties = typeof(TEntity).GetProperties();

        /// <summary>
        /// Inicializa una nueva instancia del repositorio.
        /// </summary>
        /// <param name="context">
        /// Contexto de base de datos.
        /// </param>
        protected DbRepository(DbContext context) { _context = context; }

        /// <summary>
        /// Ejecuta el comando especificado y mapea el resultado a una colección de entidades.
        /// </summary>
        /// <param name="command">
        /// Comando configurado para ejecutar un procedimiento almacenado de SQL Server.
        /// </param>
        /// <returns>
        /// Lista de entidades obtenidas.
        /// </returns>
        protected async Task<List<TEntity>> ToListAsync(SqlCommand command)
        {
            try
            {
                // Listado de Items mapeados
                List<TEntity> items = new();

                await using SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    items.Add(Map(reader, command)); // Mapear el registro
                }

                // Retornar lista mapeada
                return items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error ejecutando " + command.GetProcedureName() + ": " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Convierte el registro actual del DataReader en una instancia de la entidad.
        /// </summary>
        /// <param name="reader">
        /// Registro actual del DataReader.
        /// </param>
        /// <param name="command">
        /// Comando SQL Server asociado a la ejecución.
        /// </param>
        /// <returns>
        /// Modelo mapeado.
        /// </returns>
        private TEntity Map(SqlDataReader reader, SqlCommand command)
        {
            TEntity tEntity = new();

            foreach (PropertyInfo property in _properties)
            {
                try
                {
                    int ordinal = reader.GetOrdinal(property.Name);

                    if (!reader.IsDBNull(ordinal))
                    {
                        property.SetValue(tEntity, reader.GetValue(ordinal));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Repository.Map | " + $"Function: {command.CommandText} | " + $"Entity: {typeof(TEntity).Name} | " + $"Property: {property.Name} | " + $"{ex.Message}");
                }
            }

            return tEntity;
        }


    }

}

