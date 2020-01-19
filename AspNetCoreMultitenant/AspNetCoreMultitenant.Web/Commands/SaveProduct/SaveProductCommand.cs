using AspNetCoreMultitenant.Shared;
using AspNetCoreMultitenant.Web.Models;

namespace AspNetCoreMultitenant.Web.Commands.SaveProduct
{
    public class SaveProductCommand : CompositeCommandBase<ProductEditModel>
    {
        public SaveProductCommand(ITenantProvider tenantProvider,
                                  SaveProductToDatabaseCommand saveToDb,
                                  SaveProductImagesCommand saveImages,
                                  SaveProductThumbnailsCommand saveThumbnails,
                                  SaveAdvancedProductThumbnails saveAdvancedProductThumbnails,
                                  NotifyCustomersOfProductCommand notifyCustomers)
        {
            var tenant = tenantProvider.GetTenant();

            Children.Add(saveToDb);
            Children.Add(saveImages);

            if (tenant.UseAdvancedProductThumbnails)
            {
                Children.Add(saveAdvancedProductThumbnails);
            }
            else
            {
                Children.Add(saveThumbnails);
            }

            if (tenant.SendProductNotifications)
            {
                Children.Add(notifyCustomers);
            }
        }
    }
}