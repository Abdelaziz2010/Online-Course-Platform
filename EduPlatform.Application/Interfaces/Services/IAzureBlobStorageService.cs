
namespace EduPlatform.Application.Interfaces.Services
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadAsync(byte[] fileData, string fileName, string containerName = ""); 
    }
}
