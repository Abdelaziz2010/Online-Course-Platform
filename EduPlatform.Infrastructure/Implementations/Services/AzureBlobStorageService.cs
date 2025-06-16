using Azure.Storage.Blobs;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _containerName;
        public AzureBlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["AzureBlobStorage:ConnectionString"] ?? throw new ArgumentNullException(nameof(_connectionString));
            _containerName = _configuration["AzureBlobStorage:ContainerName"] ?? throw new ArgumentNullException(nameof(_containerName));
            _blobServiceClient = new BlobServiceClient(_connectionString);
        }
        public async Task<string> UploadFileAsync(byte[] fileData, string fileName, string containerName = "")
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(string.IsNullOrEmpty(containerName) ? _containerName : containerName);

            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = new MemoryStream(fileData))
            {
                await blobClient.UploadAsync(stream);
            }

            return blobClient.Uri.ToString();
        }
    }
}
