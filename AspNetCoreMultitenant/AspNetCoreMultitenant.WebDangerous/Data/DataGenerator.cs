using AspNetCoreMultitenant.WebDangerous.Data;

namespace AspNetCoreMultitenant.Web.Data
{
    public static class DataGenerator
    {
        public static void GenerateData(this ApplicationDbContext dbContext, int tenantId)
        {
            if(tenantId == 1)
            {
                GenerateForTenant1(dbContext);
            }

            if (tenantId == 2)
            {
                GenerateForTenant2(dbContext);
            }

            if (tenantId == 3)
            {
                GenerateForTenant3(dbContext);
            }
        }

        private static void GenerateForTenant1(ApplicationDbContext dbContext)
        {
            // Generate tenant 1 data
        }

        private static void GenerateForTenant2(ApplicationDbContext dbContext)
        {
            // Generate tenant 2 data
        }

        private static void GenerateForTenant3(ApplicationDbContext dbContext)
        {
            // Generate tenant 3 data
        }
    }
}
