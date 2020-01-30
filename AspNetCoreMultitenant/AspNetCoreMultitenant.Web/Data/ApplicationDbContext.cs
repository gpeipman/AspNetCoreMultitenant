using AspNetCoreMultitenant.Shared;
using AspNetCoreMultitenant.Shared.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AspNetCoreMultitenant.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext, IMultitenantDbContext
    {
        private readonly Tenant _tenant;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantProvider tenantProvider) : base(options)
        {
            _tenant = tenantProvider.GetTenant();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }

        public int TenantId
        {
            get { return _tenant.Id; }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_tenant.DatabaseType == 1)
            {
                optionsBuilder.UseSqlServer(_tenant.ConnectionString);
            }
            else if(_tenant.DatabaseType == 2)
            {
                optionsBuilder.UseMySql(_tenant.ConnectionString);
            }

            optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var navigation = builder.Entity<ProductCategory>()
                                    .Metadata
                                    .FindNavigation(nameof(ProductCategory.Products));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            base.OnModelCreating(builder);
        }
    }
}
