using System.IO;
using Newtonsoft.Json;

namespace AspNetCoreMultitenant.Shared.TenantSources
{
    public class FileTenantSource : ITenantSource
    {
        public Tenant[] ListTenants()
        {
            var tenants = File.ReadAllText("tenants.json");
            
            return JsonConvert.DeserializeObject<Tenant[]>(tenants);
        }
    }
}