
namespace EduPlatform.Application.Interfaces.Services
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(byte[] fileData, string fileName, string containerName = ""); 
    }
}
