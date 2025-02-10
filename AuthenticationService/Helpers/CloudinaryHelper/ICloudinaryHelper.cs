using AuthenticationService.Dtos.AuthenticationDtos;

namespace AuthenticationService.Helpers.CloudinaryHelper
{
    public interface ICloudinaryHelper
    {
        Task<string> UploadLabourProfileImageAsyn(IFormFile file);
    }
}
