using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using HighFive.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HighFive.Core.Provider
{
    public class AzureFileStorageProvider : IFileStorageProvider
    {
        private readonly ILogger _logger;
        private readonly string _constr;

        private const string ROOT_DIRECTORY = "wwwroot/";
        private const string LOCAL_FILE_DIRECTORY = "FileStorage";
        public AzureFileStorageProvider(IOptions<AzureStorageConnectionStringConfig> config,
            ILogger<AzureFileStorageProvider> logger)
        {
#if (DEBUG)
            _constr = config.Value.DefaultConnection;
#endif
#if (RELEASE)
            _constr = config.Value.ProductionConnection;
#endif

            _logger = logger;
        }

        private BlobContainerClient GetContainerClient()
        {
            // Create a client that can authenticate with a connection string
            BlobServiceClient _blobClient = new BlobServiceClient(_constr);
            var container = _blobClient.GetBlobContainerClient("testcontainer");
            container.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            return container;
        }


        public async Task<bool> DeleteFileAsync(string filename)
        {
            var container = GetContainerClient();

            try
            {
                var result = await container.DeleteBlobAsync(filename);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public Task<IEnumerable<string>> ListFileUrlsAsync(string path)
        {
            //Create a unique name for the container
            //string containerName = "quickstartblobs" + Guid.NewGuid().ToString();
            //_blobClient.
            //// Create the container and return a container client object
            //BlobContainerClient containerClient = await _blobClient.CreateBlobContainerAsync(containerName);

            //_blobClient.getcon

            throw new NotImplementedException();
        }

        public async Task<string> SaveFileAsync(string filename, string path, Stream stream, long length)
        {
            var combinedPath = path + '/' + filename;

            return await SaveFileAsync(combinedPath, stream, length);
        }

        public async Task<string> SaveFileAsync(string path, Stream stream, long length)
        {
            var container = GetContainerClient();
            var blob = container.GetBlobClient(path);
            try
            {
                await blob.UploadAsync(stream, true);
                return blob.Uri.AbsoluteUri;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return string.Empty;
            }
        }
    }
}
