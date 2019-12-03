using AspNetCoreMultitenant.Web.Commands.SaveProduct;
using AspNetCoreMultitenant.Web.Data;
using AspNetCoreMultitenant.Web.Extensions;
using AspNetCoreMultitenant.Web.FileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<ITenantProvider, FileTenantProvider>();
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            app.UseMiddleware<MissingTenantMiddleware>(Configuration["MissingTenantUrl"]);

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}