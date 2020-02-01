using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreMultitenant.Shared.TenantProviders
{
    public class WebTenantProvider : ITenantProvider
    {
        private ITenantSource _tenantSource;
        private string _host;

        public WebTenantProvider(ITenantSource tenantSource, IHttpContextAccessor accessor)
        {
            _tenantSource = tenantSource;
            _host = accessor.HttpContext.Request.Host.ToString();
        }

        public Tenant GetTenant()
        {
            return _tenantSource.ListTenants()
                                .Where(t => t.Host.ToLower() == _host.ToLower())
                                .FirstOrDefault();
        }
    }
}