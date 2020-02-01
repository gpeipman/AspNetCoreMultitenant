using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace AspNetCoreMultitenant.Shared.TenantSources
{
    public class BlobStorageTenantSource : ITenantSource, IDisposable
    {
        private const int TenantsUpdateInterval = 15 * 1000;

        private IList<Tenant> _tenants;
        private Timer _tenantsUpdateTimer;
        private string _storageConnectionString;
        private string _tenantsContainerName;
        private string _tenantsBlobName;

        public BlobStorageTenantSource(IConfiguration conf)
        {
            _storageConnectionString = conf["StorageConnectionString"];
            _tenantsContainerName = conf["TenantsContainerName"];
            _tenantsBlobName = conf["TenantsBlobName"];

            LoadTenants();

            _tenantsUpdateTimer = new Timer(o =>
            {
                Debug.WriteLine(DateTime.Now + ": updating tenants cache");

                LoadTenants();

            }, null, TenantsUpdateInterval, TenantsUpdateInterval);
        }

        public void Dispose()
        {
            _tenantsUpdateTimer.Dispose();
        }

        public Tenant[] ListTenants()
        {
            return _tenants.ToArray();
        }

        private void LoadTenants()
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_tenantsContainerName);
            var blob = container.GetBlobReference(_tenantsBlobName);

            blob.FetchAttributesAsync().GetAwaiter().GetResult();

            using (var stream = blob.OpenReadAsync().GetAwaiter().GetResult())
            using (var textReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(textReader))
            {
                _tenants = JsonSerializer.Create().Deserialize<List<Tenant>>(reader);
            }
        }
    }
}
