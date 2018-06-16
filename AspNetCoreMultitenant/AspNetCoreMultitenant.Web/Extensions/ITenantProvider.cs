using System;

namespace AspNetCoreMultitenant.Web.Extensions
{
    public interface ITenantProvider
    {
        Tenant GetTenant();
        Tenant[] ListTenants();
    }
}
