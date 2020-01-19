namespace AspNetCoreMultitenant.Shared.FileSystem
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