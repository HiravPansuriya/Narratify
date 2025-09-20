namespace Narratify.Services.Interfaces;

public interface IFileUploadService
{
    Task<string?> UploadFileAsync(IFormFile file, string directory);
}