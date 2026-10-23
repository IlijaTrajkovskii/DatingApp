
using CloudinaryDotNet.Actions; // od vamu gi zimame Cloudinary Types
namespace backendAPI.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);

    Task<DeletionResult> DeletePhotoAsync(string publicId);
}