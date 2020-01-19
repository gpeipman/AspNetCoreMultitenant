using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMultitenant.Shared.FileSystem
{
    public class LocalFileClient : IFileClient
    {
        public LocalFileClient(string rootPath)
        {
        }
    }
}
