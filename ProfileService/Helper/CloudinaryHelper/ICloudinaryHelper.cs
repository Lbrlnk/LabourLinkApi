using ProfileService.Dtos;

namespace ProfileService.Helper.CloudinaryHelper
{
    public interface ICloudinaryHelper
    {
        Task<string> UploadImageAsync(IFormFile file ,  bool ProfileImage);
        Task<bool> DeleteImageAsync(string publicId);
        string ExtractPublicIdFromUrl(string imageUrl);

    }
}
