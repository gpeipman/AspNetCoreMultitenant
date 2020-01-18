using System.Diagnostics;
using AspNetCoreMultitenant.Web.Data;
using AspNetCoreMultitenant.Web.Models;

namespace AspNetCoreMultitenant.Web.Commands.SaveProduct
{
    public class SaveProductToDatabaseCommand : ICommand<ProductEditModel>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveProductToDatabaseCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(ProductEditModel parameter)
        {
            Debug.Write("Save product to database: provider is ");
            Debug.WriteLine(_dbContext.Database.ProviderName);
        }
    }
}