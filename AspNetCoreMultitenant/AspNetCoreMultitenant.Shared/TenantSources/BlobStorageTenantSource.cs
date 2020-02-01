using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<string, Tenant> _tenants;
        private readonly Timer _tenantsUpdateTimer;
        private readonly string _storageConnectionString;
        private readonly string _tenantsContainerName;
        private readonly string _tenantsBlobName;

        public BlobStorageTenantSource(IConfiguration conf)
        {
            _tenants = new ConcurrentDictionary<string, Tenant>();
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

        public Tenant[] ListTenants()
        {
            return _tenants.Select(t => t.Value).ToArray();
        }

        private void LoadTenants()
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(_tenantsContainerName);
            var blob = container.GetBlobReference(_tenantsBlobName);
            var tenantList = (List<Tenant>)null;

            blob.FetchAttributesAsync().GetAwaiter().GetResult();

            using (var stream = blob.OpenReadAsync().GetAwaiter().GetResult())
            using (var textReader = new StreamReader(stream))
            using (var reader = new JsonTextReader(textReader))
            {
                tenantList = JsonSerializer.Create().Deserialize<List<Tenant>>(reader);
            }

            var currentKeys = _tenants.Keys;

            // remove invalid keys
            foreach (var key in currentKeys)
            {
                if (tenantList.Any(t => t.Host == key))
                {
                    continue;
                }

                _tenants.TryRemove(key, out _);
            }

            // add and update tenants
            foreach(var tenant in tenantList)
            {
                var host = tenant.Host.ToLower();

                _tenants.AddOrUpdate(host, tenant, (k, v) => tenant);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tenantsUpdateTimer.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
