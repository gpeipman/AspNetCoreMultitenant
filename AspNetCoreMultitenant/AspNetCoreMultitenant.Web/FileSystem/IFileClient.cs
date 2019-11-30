using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMultitenant.Web.FileSystem
{
    public interface IFileClient
    {
    }

    public class AzureBlobStorageFileClient : IFileClient
    {
        public AzureBlobStorageFileClient(string connectionString)
        {
        }
    }

    public class GoogleDriveFileClient : IFileClient
    {
        public GoogleDriveFileClient(string connectionString)
        {
        }
    }
}
