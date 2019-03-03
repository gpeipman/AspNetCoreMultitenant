using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreMultitenant.Web.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyModel;

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

            foreach (var type in GetEntityTypes())
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { builder });
            }

            base.OnModelCreating(builder);
        }

        public int TenantId
        {
            get
            {
                if(_tenant == null)
                {
                    return -1;
                }

                return _tenant.Id;
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }

        #region EntityTypes
        private static IList<Type> _entityTypeCache;
        private static IList<Type> GetEntityTypes()
        {
            if (_entityTypeCache != null)
            {
                return _entityTypeCache.ToList();
            }

            _entityTypeCache = (from a in GetReferencingAssemblies()
                                from t in a.DefinedTypes
                                where t.BaseType == typeof(BaseEntity)
                                select t.AsType()).ToList();

            return _entityTypeCache;
        }

        private static IEnumerable<Assembly> GetReferencingAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                { }
            }
            return assemblies;
        }

        static readonly MethodInfo SetGlobalQueryMethod = typeof(ApplicationDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                                                      .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasKey(e => e.Id);
            //Debug.WriteLine("Adding global query for: " + typeof(T));
            builder.Entity<T>().HasQueryFilter(e => e.TenantId == _tenant.Id);
        }
        #endregion

        #region DefensiveOps
        public override int SaveChanges()
        {
            ThrowIfMultipleTenants();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ThrowIfMultipleTenants();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfMultipleTenants();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfMultipleTenants();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void ThrowIfMultipleTenants()
        {
            var ids = (from e in ChangeTracker.Entries()
                       where e.Entity is BaseEntity
                       select ((BaseEntity)e.Entity).TenantId)
                       .Distinct()
                       .ToList();

            if (ids.Count == 0)
            {
                return;
            }

            if (ids.Count > 1)
            {
                throw new CrossTenantUpdateException(ids);
            }

            if (ids.First() != _tenant.Id)
            {
                throw new CrossTenantUpdateException(ids);
            }
        }
        #endregion
    }
}
