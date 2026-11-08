using BBoxSoft.Domain.Models.Universe;
using BBoxSoft.Infrastructure.Database.Connection;
using BBoxSoft.Infrastructure.Database.DataAccess.Universe;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBoxSoft.Application.Services.Universe
{
    public class UnvTenantService : BaseService
    {
        public async Task<List<UnvTenantModel>> GetAllAsync(string cnn, bool showDisabled)
        {
            try
            {
                return await GetDataAccess<DaUnvTenant>(cnn).GetAllAsync(showDisabled);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await DisposeContextAsync();
            }
        }




    }
}
