using System.Linq;
using AspNetCoreMultitenant.WebDangerous.Data;
using AspNetCoreMultitenant.WebDangerous.TenantProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMultitenant.WebDangerous
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => { });
            services.AddHttpContextAccessor();
            services.AddMvc();
            services.AddControllersWithViews();

            services.AddScoped<ITenantProvider, FileTenantProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var options = new DbContextOptions<ApplicationDbContext>();
            var provider = new FileTenantProvider();

            foreach (var tenant in provider.ListTenants())
            {
                provider.SetHostName(tenant.Host);

                using (var dbContext = new ApplicationDbContext(options, provider))
                {
                    dbContext.Database.EnsureCreated();

                    if (dbContext.Products.Count(p => p.TenantId == tenant.Id) == 0)
                    {
                        dbContext.GenerateData(tenant.Id);
                    }
                }
            }
        }
    }
}
