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
            var tvCategory = new ProductCategory { Name = "TV" };
            var computersCategory = new ProductCategory { Name = "Computers" };

            dbContext.Categories.Add(tvCategory);
            dbContext.Categories.Add(computersCategory);

            dbContext.Products.Add(new Product
            {
                Name = "55\" Ultra HD LED LCD - teler, Philips",
                Description = "Huge TV for private houses",
                Category = tvCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "49\" Ultra HD LED LCD-teler Sony",
                Description = "Another huge TV",
                Category = tvCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "Lenovo Miix 320",
                Description = "Hybrid laptop/tablet for kids and home users",
                Category = computersCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "Samsung Galaxy Tab S3 / WiFi, LTE",
                Description = "Powerful business class tablet",
                Category = computersCategory
            });

            dbContext.SaveChanges();
        }

        private static void GenerateForTenant2(ApplicationDbContext dbContext)
        {
            var actionCategory = new ProductCategory { Name = "Action" };
            var romanceCategory = new ProductCategory { Name = "Romance" };

            dbContext.Categories.Add(actionCategory);
            dbContext.Categories.Add(romanceCategory);

            dbContext.Products.Add(new Product
            {
                Name = "Deadpool 2",
                Description = "Foul-mouthed mutant mercenary Wade Wilson (AKA. Deadpool), brings together a team of fellow mutant rogues to protect a young boy with supernatural abilities from the brutal, time-traveling cyborg, Cable.",
                Category = actionCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "Cobra Kai",
                Description = "Set thirty years after the events of the 1984 All Valley Karate Tournament, the series focuses on Johnny Lawrence reopening the Cobra Kai dojo, which causes his rivalry with Daniel LaRusso to be reignited.",
                Category = actionCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "The Kissing Booth",
                Description = "A high school student is forced to confront her secret crush at a kissing booth.",
                Category = romanceCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "Shadowhunters: The Mortal Instruments",
                Description = "After her mother disappears, Clary must venture into the dark world of demon hunting, and embrace her new role among the Shadowhunters.",
                Category = romanceCategory
            });

            dbContext.SaveChanges();
        }

        private static void GenerateForTenant3(ApplicationDbContext dbContext)
        {
            var winesCategory = new ProductCategory { Name = "Wines" };
            var beersCategory = new ProductCategory { Name = "Beers" };

            dbContext.Categories.Add(winesCategory);
            dbContext.Categories.Add(beersCategory);

            dbContext.Products.Add(new Product
            {
                Name = "Cramele Tataran ITCamp Edition",
                Description = "Special edition for ITCamp 2018 speakers. Cheers, Romania!",
                Category = winesCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "Kagor Osobхi",
                Description = "Uuh.....",
                Category = winesCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "La Trappe Bockbier (7%)",
                Description = "La Trappe Bock beer is an all-malt, multigrain Bock beer that undergoes secondary fermentation. La Trappe Bock beer is the only Trappist Bock beer in the world. It is brewed with nothing but natural ingredients and goes through a light secondary fermentation in the bottle.",
                Category = beersCategory
            });

            dbContext.Products.Add(new Product
            {
                Name = "La Trappe Quadrupel (10%)",
                Description = "In 1991, The Koningshoeven brewery baptized her beloved Quadrupel under the approving glance of the monks. A closely guarded recipe became reality, and the first Quadrupel beer in the world was born.",
                Category = beersCategory
            });

            dbContext.SaveChanges();
        }
    }
}