using Microsoft.AspNetCore.Mvc;

namespace BBoxSoft.Api.Controllers
{
    public class BaseController : Controller
    {
        //Cadena de Conexión a Base de Datos
        protected readonly string _universeConnectionString;

        public BaseController(IConfiguration iConfiguration)
        {
            _universeConnectionString = iConfiguration.GetConnectionString("UniverseConnectionString") ?? throw new InvalidOperationException("No se encontró la cadena de conexión principal");
        }

    }
}
