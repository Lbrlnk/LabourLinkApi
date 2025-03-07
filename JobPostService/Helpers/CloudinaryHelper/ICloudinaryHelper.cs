namespace JobPostService.Helpers.CloudinaryHelper
{
	public interface ICloudinaryHelper
	{
		Task<string> UploadImage(IFormFile file);
	}
}
