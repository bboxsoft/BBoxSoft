using BBoxSoft.Domain.Models.Universe;
using BBoxSoft.Infrastructure.Database.Connection;
using BBoxSoft.Infrastructure.Extensions;
using BBoxSoft.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;

namespace BBoxSoft.Infrastructure.Database.DataAccess.Universe
{
    public class DaUnvTenant : DbRepository<UnvTenantModel>
    {
        public DaUnvTenant(DbContext context) : base(context) { }

        public async Task<List<UnvTenantModel>> GetAllAsync(bool showDisabled)
        {
            await using SqlCommand command = await _context.CreateCommandAsync("dfTenant_GetAll");

            command.AddParameter("pShowDisabled", showDisabled);

            return await this.ToListAsync(command);
        }




    }
}
