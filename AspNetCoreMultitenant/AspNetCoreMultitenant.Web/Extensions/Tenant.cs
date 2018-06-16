namespace AspNetCoreMultitenant.Web.Extensions
{
    public class Tenant
    {
        public int Id { get; set; }
        public int DatabaseType { get; set; }
        public string Host { get; set; }
        public string ConnectionString { get; set; }
        public string StorageConnectionString { get; set; }
        public string Name { get; set; }
    }
}
