using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace AspNetCoreMultitenant.Web.Extensions
{
    public class BlobStorageTenantProvider : ITenantProvider
    {
        private static IList<Tenant> _tenants;
        private string _host;

        public BlobStorageTenantProvider(IHttpContextAccessor accessor, IConfiguration conf)
        {
            if (_tenants == null)
            {
                LoadTenants(conf["StorageConnectionString"], conf["TenantsContainerName"], conf["TenantsBlobName"]);
            }

            _host = accessor.HttpContext.Request.Host.Value;
        }

        private void LoadTenants(string connStr, string containerName, string blobName)
        {
            var storageAccount = CloudStorageAccount.Parse(connStr);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var blob = container.GetBlobReference(blobName);

            blob.FetchAttributesAsync().GetAwaiter().GetResult();

            var fileBytes = new byte[blob.Properties.Length];

            using (var stream = blob.OpenReadAsync().GetAwaiter().GetResult())
            using (var textReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(textReader))
            {
                _tenants = JsonSerializer.Create().Deserialize<List<Tenant>>(reader);
            }
        }

        public Tenant GetTenant()
        {
            return _tenants.FirstOrDefault(t => t.Host.ToLower() == _host.ToLower());
        }

        public Tenant[] ListTenants()
        {
            return _tenants.ToArray();
        }
    }
}
