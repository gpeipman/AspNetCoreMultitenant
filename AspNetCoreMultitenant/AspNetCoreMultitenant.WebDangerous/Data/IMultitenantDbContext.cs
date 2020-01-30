namespace AspNetCoreMultitenant.WebDangerous.Data
{
    public interface IMultitenantDbContext
    {
        public int TenantId { get; }
    }
}
