using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMultitenant.Web.Models;

namespace AspNetCoreMultitenant.Web.Commands.SaveProduct
{
    public class SaveAdvancedProductThumbnails : ICommand<ProductEditModel>
    {
        public void Execute(ProductEditModel parameter)
        {
            Debug.WriteLine("Save advanced thumbnails");
        }
    }
}
