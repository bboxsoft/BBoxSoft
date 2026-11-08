using BBoxSoft.Application.Services;
using BBoxSoft.Infrastructure.Database.Connection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBoxSoft.Application.BusinessLogic
{
    internal class class1
    {
        public void methodEx(string cnn)
        {
            //DbContext context = new(new ConnectionFactory(cnn));

            //await context.BeginTransactionAsync();

            //UnvTenantService tenantService = new();
            //tenantService.UseDbContext(context);

            //await tenantService.GetAllAsync("", false);

            //// ...

            //await context.CommitAsync();
            //await context.DisposeAsync();
        }

    }
}
