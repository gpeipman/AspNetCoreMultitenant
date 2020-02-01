using System.Linq;
using AspNetCoreMultitenant.Shared;
using AspNetCoreMultitenant.Shared.FileSystem;
using AspNetCoreMultitenant.Shared.TenantProviders;
using AspNetCoreMultitenant.Shared.TenantSources;
using AspNetCoreMultitenant.Web.Commands.SaveProduct;
using AspNetCoreMultitenant.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMultitenant.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options => {});
            services.AddIdentityCore<IdentityUser>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddSignInManager();

            services.AddHttpContextAccessor();
            services.AddMvc();
            services.AddControllersWithViews();

            services.AddSingleton<ITenantSource, FileTenantSource>();
            services.AddScoped<ITenantProvider, WebTenantProvider>();
            services.AddScoped<SaveProductCommand>();
            services.AddScoped<SaveProductImagesCommand>();
            services.AddScoped<SaveProductThumbnailsCommand>();
            services.AddScoped<SaveAdvancedProductThumbnails>();
            services.AddScoped<SaveProductToDatabaseCommand>();
            services.AddScoped<NotifyCustomersOfProductCommand>();

            services.AddScoped<IFileClient>(service =>
            {
                var provider = service.GetRequiredService<ITenantProvider>();
                var tenant = provider.GetTenant();

                if(tenant.StorageType == "GoogleDrive")
                {
                    return new GoogleDriveFileClient(tenant.ConnectionString);
                }

                if(tenant.StorageType == "AzureBlob")
                {
                    return new AzureBlobStorageFileClient(tenant.ConnectionString);
                }

                return null;
            });
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<MissingTenantMiddleware>(Configuration["MissingTenantUrl"]);

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            var options = new DbContextOptions<ApplicationDbContext>();
            var source = new FileTenantSource();
            var provider = new ControllableTenantProvider();

            foreach (var tenant in source.ListTenants())
            {
                provider.Tenant = tenant;

                using (var dbContext = new ApplicationDbContext(options, provider))
                {
                    dbContext.Database.EnsureCreated();

                    if (dbContext.Products.Count() == 0)
                    {
                        dbContext.GenerateData(tenant.Id);
                    }
                }
            }
        }
    }
}