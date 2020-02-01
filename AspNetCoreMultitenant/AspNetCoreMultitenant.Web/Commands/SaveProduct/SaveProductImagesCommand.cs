using System.Diagnostics;
using AspNetCoreMultitenant.Shared.FileSystem;
using AspNetCoreMultitenant.Web.Models;

namespace AspNetCoreMultitenant.Web.Commands.SaveProduct
{
    public class SaveProductImagesCommand : ICommand<ProductEditModel>
    {
        private readonly IFileClient _fileClient;

        public SaveProductImagesCommand(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        public void Execute(ProductEditModel parameter)
        {
            Debug.Write("Save product images: file client is ");
            Debug.WriteLine(_fileClient.GetType());
        }
    }
}
