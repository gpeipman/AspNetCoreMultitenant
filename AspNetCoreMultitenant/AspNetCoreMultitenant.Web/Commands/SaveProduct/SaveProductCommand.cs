using AspNetCoreMultitenant.Web.Models;

namespace AspNetCoreMultitenant.Web.Commands.SaveProduct
{
    public class SaveProductCommand : CompositeCommandBase<ProductEditModel>
    {
        public SaveProductCommand(SaveProductToDatabaseCommand saveToDb,
                                  SaveProductImagesCommand saveImages,
                                  SaveProductThumbnailsCommand saveThumbnails,
                                  NotifyCustomersOfProductCommand notifyCustomers)
        {
            Children.Add(saveToDb);
            Children.Add(saveImages);
            Children.Add(saveThumbnails);
            Children.Add(notifyCustomers);
        }
    }
}