using AspNetCoreMultitenant.Web.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AspNetCoreMultitenant.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly Tenant _tenant;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantProvider tenantProvider) : base(options)
        {
            _tenant = tenantProvider.GetTenant();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
        }

        public int TenantId
        {
            get { return _tenant.Id; }
        }
    }
}
