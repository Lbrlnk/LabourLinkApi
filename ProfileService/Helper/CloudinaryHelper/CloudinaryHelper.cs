using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
namespace ProfileService.Helper.CloudinaryHelper
{
    public class CloudinaryHelper : ICloudinaryHelper
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryHelper(IConfiguration configuration)
        {
            var cloudName = "djvap1ftu";
            var apiKey = "366328664937922";
            var apiSecret = "rFJ6_v6Ns6Zlv1gC_TDmdB42Nhk";

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file, bool ProfileImage)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty", nameof(file));
            }
            using var stream = file.OpenReadStream();
            ImageUploadParams uploadParams;
            if (ProfileImage)
            {
                uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "LabourLinkProfiles",
                    Transformation = new Transformation().Quality("auto").FetchFormat("auto")
                };
            }
            else
            {
                uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "LabourWorkImages",
                    Transformation = new Transformation().Quality("auto").FetchFormat("auto")
                };
            }
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");
            }
            return uploadResult.SecureUrl.ToString();
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);
                if (result.Result == "ok")
                {
                    return true;
                }
                if (result.Error != null)
                {
                    throw new Exception($"Cloudinary image deletion failed: {result.Error.Message}");
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete image with public ID: {publicId}. Error: {ex.Message}", ex);
            }
        }

        public string ExtractPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.Segments;
            var fileName = segments.Last(); // Get the last segment (e.g., 'imageName.jpg')
            var publicId = fileName.Substring(0, fileName.LastIndexOf(".")); // Remove file extension
            return publicId;
        }
    }
}