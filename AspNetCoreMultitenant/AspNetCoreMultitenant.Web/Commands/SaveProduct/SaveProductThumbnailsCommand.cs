using System.Diagnostics;
using AspNetCoreMultitenant.Shared.FileSystem;
using AspNetCoreMultitenant.Web.Models;

namespace AspNetCoreMultitenant.Web.Commands.SaveProduct
{
    public class SaveProductThumbnailsCommand : ICommand<ProductEditModel>
    {
        private readonly IFileClient _fileClient;

        public SaveProductThumbnailsCommand(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        public void Execute(ProductEditModel parameter)
        {
            Debug.Write("Save product thumbnails: file client is ");
            Debug.WriteLine(_fileClient.GetType());
        }
    }
}