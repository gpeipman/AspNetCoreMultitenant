namespace AspNetCoreMultitenant.Shared
{
    public interface ITenantProvider
    {
        Tenant GetTenant();
    }
}