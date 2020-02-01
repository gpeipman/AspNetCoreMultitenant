namespace AspNetCoreMultitenant.WebDangerous.Data
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
        }

        private static void GenerateForTenant1(ApplicationDbContext dbContext)
        {
            var tvCategory = new ProductCategory { Name = "TV", TenantId = 1 };
            var computersCategory = new ProductCategory { Name = "Computers", TenantId = 1 };

            dbContext.Categories.Add(tvCategory);
            dbContext.Categories.Add(computersCategory);

            dbContext.Products.Add(new Product
            {
                Name = "55\" Ultra HD LED LCD - teler, Philips",
                Description = "Huge TV for private houses",
                Category = tvCategory,
                TenantId = 1
            });

            dbContext.Products.Add(new Product
            {
                Name = "49\" Ultra HD LED LCD-teler Sony",
                Description = "Another huge TV",
                Category = tvCategory,
                TenantId = 1
            });

            dbContext.Products.Add(new Product
            {
                Name = "Lenovo Miix 320",
                Description = "Hybrid laptop/tablet for kids and home users",
                Category = computersCategory,
                TenantId = 1
            });

            dbContext.Products.Add(new Product
            {
                Name = "Samsung Galaxy Tab S3 / WiFi, LTE",
                Description = "Powerful business class tablet",
                Category = computersCategory,
                TenantId = 1
            });

            dbContext.SaveChanges();
        }

        private static void GenerateForTenant2(ApplicationDbContext dbContext)
        {
            var actionCategory = new ProductCategory { Name = "Action", TenantId = 2 };
            var romanceCategory = new ProductCategory { Name = "Romance", TenantId = 2 };

            dbContext.Categories.Add(actionCategory);
            dbContext.Categories.Add(romanceCategory);

            dbContext.Products.Add(new Product
            {
                Name = "Deadpool 2",
                Description = "Foul-mouthed mutant mercenary Wade Wilson (AKA. Deadpool), brings together a team of fellow mutant rogues to protect a young boy with supernatural abilities from the brutal, time-traveling cyborg, Cable.",
                Category = actionCategory, 
                TenantId = 2
            });

            dbContext.Products.Add(new Product
            {
                Name = "Cobra Kai",
                Description = "Set thirty years after the events of the 1984 All Valley Karate Tournament, the series focuses on Johnny Lawrence reopening the Cobra Kai dojo, which causes his rivalry with Daniel LaRusso to be reignited.",
                Category = actionCategory, 
                TenantId = 2
            });

            dbContext.Products.Add(new Product
            {
                Name = "The Kissing Booth",
                Description = "A high school student is forced to confront her secret crush at a kissing booth.",
                Category = romanceCategory, 
                TenantId = 2
            });

            dbContext.Products.Add(new Product
            {
                Name = "Shadowhunters: The Mortal Instruments",
                Description = "After her mother disappears, Clary must venture into the dark world of demon hunting, and embrace her new role among the Shadowhunters.",
                Category = romanceCategory, 
                TenantId = 2
            });

            dbContext.SaveChanges();
        }
    }
}
