using System.Diagnostics;
using System.Linq;
using AspNetCoreMultitenant.Web.Commands.SaveProduct;
using AspNetCoreMultitenant.Web.Data;
using AspNetCoreMultitenant.Web.Extensions;
using AspNetCoreMultitenant.Web.FileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

            //var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            //using (var serviceScope = serviceScopeFactory.CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetService<LasteDbContext>();
            //    dbContext.Database.EnsureCreated();

            //    if (dbContext.Invoices.Count() == 0)
            //    {
            //        for (var i = 0; i < 11; i++)
            //        {
            //            //dbContext.Invoices.Add(invoice);
            //        }
            //        dbContext.SaveChanges();
            //    }
            //}

            var options = new DbContextOptions<ApplicationDbContext>();
            var provider = new FileTenantProvider();

            foreach (var tenant in provider.ListTenants())
            {
                provider.SetHostName(tenant.Host);

                using (var dbContext = new ApplicationDbContext(options, provider))
                {
                    dbContext.Database.EnsureCreated();

                    if (dbContext.Products.Count() == 0)
                    {
                        // Lisa andmed andmebaasi
                        dbContext.GenerateData(tenant.Id);
                    }
                }
            }
        }
    }
}