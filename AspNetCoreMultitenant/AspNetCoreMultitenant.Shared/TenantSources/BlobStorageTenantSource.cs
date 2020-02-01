using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace AspNetCoreMultitenant.Shared.TenantSources
{
    public class BlobStorageTenantSource : ITenantSource
    {
        private static IList<Tenant> _tenants;

        public BlobStorageTenantSource(IConfiguration conf)
        {
            if (_tenants == null)
            {
                LoadTenants(conf["StorageConnectionString"], conf["TenantsContainerName"], conf["TenantsBlobName"]);
            }
        }

        private void LoadTenants(string connStr, string containerName, string blobName)
        {
            var storageAccount = CloudStorageAccount.Parse(connStr);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var blob = container.GetBlobReference(blobName);

            blob.FetchAttributesAsync().GetAwaiter().GetResult();

            using (var stream = blob.OpenReadAsync().GetAwaiter().GetResult())
            using (var textReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(textReader))
            {
                _tenants = JsonSerializer.Create().Deserialize<List<Tenant>>(reader);
            }
        }

        public Tenant[] ListTenants()
        {
            return _tenants.ToArray();
        }
    }
}
