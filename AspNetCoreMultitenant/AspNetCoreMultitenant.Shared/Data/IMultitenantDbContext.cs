namespace AspNetCoreMultitenant.Shared.Data
{
    public interface IMultitenantDbContext
    {
        int TenantId { get; }
    }
}