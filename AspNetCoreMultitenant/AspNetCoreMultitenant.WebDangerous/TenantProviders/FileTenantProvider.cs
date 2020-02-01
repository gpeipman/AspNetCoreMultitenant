    using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AspNetCoreMultitenant.WebDangerous.TenantProviders
{
    public class FileTenantProvider : ITenantProvider
    {
        private static Tenant[] _tenants = new Tenant[] { };
        private static DateTime _lastUpdate = DateTime.MinValue;
        private static object _locker = new object();

        private string _host;

        public FileTenantProvider(IHttpContextAccessor accessor)
        {
            _host = accessor.HttpContext.Request.Host.ToString();
        }

        public FileTenantProvider()
        {
        }

        public Tenant GetTenant()
        {
            LoadTenants();
            
            return _tenants.FirstOrDefault(t => t.Host.ToLower() == _host.ToLower());
        }

        public void SetHostName(string host)
        {
            _host = host;
        }

        private static void LoadTenants()
        {
            if(_tenants != null && _tenants.Length > 0 && CacheAge <= 15)
            {
                Debug.WriteLine(CacheAge + ": not expired");
                return;
            }

            if(!Monitor.TryEnter(_locker))
            {
                return;
            }

            Debug.WriteLine(CacheAge + ": expired");

            try
            {
                if(CacheAge <= 15)
                {
                    return;
                }

                var tenants = File.ReadAllText("tenants.json");
                _tenants = JsonConvert.DeserializeObject<Tenant[]>(tenants);
                _lastUpdate = DateTime.Now;

                Debug.WriteLine("CACHE UPDATE");
            }
            finally
            {
                Monitor.Exit(_locker);
            }
        }

        private static double CacheAge
        {
            get { return (DateTime.Now - _lastUpdate).TotalSeconds; }
        }

        public Tenant[] ListTenants()
        {
            LoadTenants();

            return _tenants;
        }
    }
}
