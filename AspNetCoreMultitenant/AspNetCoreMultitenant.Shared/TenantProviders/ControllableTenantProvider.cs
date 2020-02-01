namespace AspNetCoreMultitenant.Shared.TenantProviders
{
    public class ControllableTenantProvider : ITenantProvider
    {
        public Tenant Tenant { get; set; }

        public Tenant GetTenant()
        {
            return Tenant;
        }
    }
}
