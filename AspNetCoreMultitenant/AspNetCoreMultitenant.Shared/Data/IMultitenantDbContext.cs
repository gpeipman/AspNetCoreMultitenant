namespace AspNetCoreMultitenant.Shared.Data
{
    public interface IMultitenantDbContext
    {
        public int TenantId { get; }
    }
}
