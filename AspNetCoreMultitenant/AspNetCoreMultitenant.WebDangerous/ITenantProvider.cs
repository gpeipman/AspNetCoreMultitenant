namespace AspNetCoreMultitenant.WebDangerous
{
    public interface ITenantProvider
    {
        Tenant GetTenant();
        Tenant[] ListTenants();
    }
}
