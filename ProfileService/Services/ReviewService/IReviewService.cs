using ProfileService.Dtos;
using ProfileService.Helpers.ApiResponse;

namespace ProfileService.Services.ReviewService
{
	public interface IReviewService
	{
		Task<ApiResponse<ReviewShowDto>> AddReview(AddReview1 review, IFormFile image, Guid userid);
		Task<ApiResponse<List<ReviewShowDto>>> ShowReviews(Guid Labourid);
		Task<ApiResponse<ReviewShowDto>> UpdateReview(Guid reviewId, AddReview1 updatedReview, IFormFile? image, Guid employerId);
		Task<ApiResponse<string>> ChangeStatusAsync(Guid reviewid, Guid userid);
		Task<ApiResponse<List<int>>> GetRating(Guid Labourid);
	}
}
