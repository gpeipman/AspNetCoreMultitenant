namespace AspNetCoreMultitenant.Shared
{
    public interface ITenantSource
    {
        Tenant GetTenant();
        Tenant[] ListTenants();
    }
}
