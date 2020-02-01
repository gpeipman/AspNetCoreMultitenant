namespace AspNetCoreMultitenant.Shared
{
    public interface ITenantSource
    {
        Tenant[] ListTenants();
    }
}
