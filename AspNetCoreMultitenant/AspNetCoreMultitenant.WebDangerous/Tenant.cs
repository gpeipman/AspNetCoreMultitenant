namespace AspNetCoreMultitenant.WebDangerous
{
    public class Tenant
    {
        public int Id { get; set; }
        public int DatabaseType { get; set; }
        public string Host { get; set; }
        public string ConnectionString { get; set; }
        public string Name { get; set; }

        public string StorageType { get; set; }
        public string StorageConnectionString { get; set; }

        public bool UseAdvancedProductThumbnails { get; set; }
        public bool SendProductNotifications { get; set; }
    }
}