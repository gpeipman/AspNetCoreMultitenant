using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AspNetCoreMultitenant.Web.Data
{
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            if (context is ApplicationDbContext dynamicContext)
            {
                return new { dynamicContext.TenantId };
            }

            throw new Exception("Unknown DBContext type");
        }
    }
}
