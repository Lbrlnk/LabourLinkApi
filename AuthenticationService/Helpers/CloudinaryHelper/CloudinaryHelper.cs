using AuthenticationService.Dtos.AuthenticationDtos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AuthenticationService.Helpers.CloudinaryHelper
{
    public class CloudinaryHelper : ICloudinaryHelper
    {

        private readonly Cloudinary _cloudinary;

        public CloudinaryHelper(IConfiguration configuration)
        {
            var cloudName = configuration["CLOUDINARY_CLOUDNAME"];
            var apiKey = configuration["CLOUDINARY_APIKEY"];
            var apiSecret = configuration["CLOUDINARY_API_SECRET"];
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadLabourProfileImageAsyn(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {

                throw new ArgumentException("File cannot be null or empty", nameof(file));
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "KaalcharakkImageStor",
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString();

        }
    }
}
