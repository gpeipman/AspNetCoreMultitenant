using System;
using System.Collections.Generic;

namespace AspNetCoreMultitenant.Web.Extensions
{
    public class CrossTenantUpdateException : ApplicationException
    {
        public IList<int> TenantIds { get; private set; }

        public CrossTenantUpdateException(IList<int> tenantIds)
        {
            TenantIds = tenantIds;
        }
    }
}