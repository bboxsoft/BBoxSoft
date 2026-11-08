using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace BBoxSoft.Infrastructure.Extensions
{
    /// <summary>
    /// Métodos de extensión para facilitar el uso de
    /// <see cref="SqlCommand"/>.
    /// </summary>
    public static class SqlCommandExtensions
    {

        /// <summary>
        /// Almacena metadatos asociados a una instancia de SqlCommand.
        /// </summary>
        private class CommandMetadata
        {
            /// <summary>
            /// Nombre del procedimiento almacenado de SQL Server asociado al comando.
            /// </summary>
            public string? ProcedureName { get; set; }
        }

        /// <summary>
        /// Almacena los metadatos asociados a cada instancia de SqlCommand.
        /// </summary>
        private static readonly ConditionalWeakTable<SqlCommand, CommandMetadata> _metadata = new();

        /// <summary>
        /// Asocia el nombre de un procedimiento almacenado de SQL Server con el comando especificado.
        /// </summary>
        /// <param name="command">Comando al que se asociará el procedimiento.</param>
        /// <param name="procedureName">Nombre del procedimiento almacenado.</param>
        public static void SetProcedureName(this SqlCommand command, string procedureName)
        {
            _metadata.GetOrCreateValue(command).ProcedureName = procedureName;
        }

        /// <summary>
        /// Obtiene el nombre del procedimiento almacenado de SQL Server asociado al comando.
        /// </summary>
        /// <param name="command">Comando del cual se obtendrá el procedimiento asociado.</param>
        /// <returns>Nombre del procedimiento almacenado o <c>null</c> si no existe.</returns>
        public static string? GetProcedureName(this SqlCommand command)
        {
            return _metadata.TryGetValue(command, out CommandMetadata? metadata) ? metadata.ProcedureName : null;
        }

        /// <summary>
        /// Agrega un parámetro al comando.
        ///
        /// Si el valor es <c>null</c>, se envía
        /// <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="command">
        /// Comando SQL Server.
        /// </param>
        /// <param name="parameterName">
        /// Nombre del parámetro.
        /// </param>
        /// <param name="value">
        /// Valor del parámetro.
        /// </param>
        public static void AddParameter(this SqlCommand command, string parameterName, object? value)
        {
            command.Parameters.AddWithValue(parameterName, value ?? DBNull.Value);
        }


    }

}
