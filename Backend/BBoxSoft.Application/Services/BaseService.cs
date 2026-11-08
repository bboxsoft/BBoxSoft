using BBoxSoft.Infrastructure.Database.Connection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBoxSoft.Application.Services
{
    /// <summary>
    /// Clase base para todos los servicios de la aplicación.
    /// Administra el ciclo de vida del contexto de base de datos.
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// Contexto de base de datos utilizado por el servicio.
        /// </summary>
        protected DbContext? _dbContext { get; private set; }

        /// <summary>
        /// Indica si el contexto fue creado por este servicio.
        /// Si es verdadero, el servicio es responsable de liberarlo.
        /// </summary>
        private bool _ownsContext;

        /// <summary>
        /// Asigna un contexto existente al servicio.
        /// El servicio utilizará este contexto pero no será responsable
        /// de liberarlo.
        /// </summary>
        /// <param name="dbContext">
        /// Contexto existente.
        /// </param>
        public void UseDbContext(DbContext dbContext)
        {
            if (_dbContext != null) { throw new InvalidOperationException("El servicio ya tiene un DbContext asignado."); }

            _dbContext = dbContext;
            _ownsContext = false;
        }

        /// <summary>
        /// Obtiene el contexto de base de datos.
        /// Si el servicio no posee uno, crea una nueva instancia.
        /// </summary>
        /// <param name="connectionString">
        /// Cadena de conexión.
        /// </param>
        /// <returns>
        /// Contexto de base de datos.
        /// </returns>
        protected DbContext GetDbContext(string connectionString)
        {
            if (_dbContext != null) { return _dbContext; }  //Si el objeto ya tiene dbContext, se retorna el existente

            //Si no existe dbContext, se crea uno nuevo
            _dbContext = new DbContext(new ConnectionFactory(connectionString));
            _ownsContext = true;

            return _dbContext;
        }

        /// <summary>
        /// Obtiene una instancia del DataAccess solicitado utilizando el contexto
        /// de base de datos actual.
        /// </summary>
        /// <typeparam name="TDataAccess">
        /// Tipo del DataAccess.
        /// </typeparam>
        /// <param name="connectionString">
        /// Cadena de conexión.
        /// </param>
        /// <returns>
        /// Instancia del DataAccess.
        /// </returns>
        protected TDataAccess GetDataAccess<TDataAccess>(string connectionString) where TDataAccess : class
        {
            DbContext dbCcontext = GetDbContext(connectionString);
            return (TDataAccess)Activator.CreateInstance(typeof(TDataAccess), dbCcontext)!;
        }

        /// <summary>
        /// Libera el contexto únicamente si fue creado por el servicio.
        /// </summary>
        protected async Task DisposeContextAsync()
        {
            if (!_ownsContext) { return; }  //Si el dbContext no le pertenece al Service, entonces salimos del método

            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync();
                _dbContext = null;
            }

            _ownsContext = false;
        }

    }
}
