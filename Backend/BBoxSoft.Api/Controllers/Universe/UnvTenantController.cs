using BBoxSoft.Application.Services.Universe;
using Microsoft.AspNetCore.Mvc;

namespace BBoxSoft.Api.Controllers.Universe
{
    [ApiController]
    [Route("api/Tenant")]
    public class UnvTenantController : BaseController
    {
        private readonly UnvTenantService _unvTenantService;

        public UnvTenantController(IConfiguration configuration, UnvTenantService unvTenantService) : base(configuration)
        {
            _unvTenantService = unvTenantService;
        }

        [HttpGet]
        [Route("api/Tenant/GetAll/{ShowDisabled}")]
        public async Task<IActionResult> GetAll(bool ShowDisabled)
        {
            object objStatus = await _unvTenantService.GetAllAsync(_universeConnectionString, ShowDisabled);

            return StatusCode(200, objStatus);
        }

    }
}
