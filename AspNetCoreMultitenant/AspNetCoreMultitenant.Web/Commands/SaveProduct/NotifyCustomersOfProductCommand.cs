using System.Diagnostics;
using AspNetCoreMultitenant.Web.Models;

namespace AspNetCoreMultitenant.Web.Commands.SaveProduct
{
    public class NotifyCustomersOfProductCommand : ICommand<ProductEditModel>
    {
        public void Execute(ProductEditModel parameter)
        {
            Debug.WriteLine("Notify customers of new product");
        }
    }
}