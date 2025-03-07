using Microsoft.AspNetCore.Http.HttpResults;
using ProfileService.Dtos;
using ProfileService.Helper.CloudinaryHelper;
using ProfileService.Helpers.ApiResponse;
using ProfileService.Models;
using ProfileService.Repositories.EmployerRepository;
using ProfileService.Repositories.ReviewRepository;

namespace ProfileService.Services.ReviewService
{
	public class ReviewService: IReviewService
	{
		private readonly IReviewRepository _repository;
		private readonly ICloudinaryHelper _cloudinary;
		private readonly IEmployerRepository _employer;
		public ReviewService(IReviewRepository repository,ICloudinaryHelper cloudinary, IEmployerRepository employer)
		{
			_repository = repository;
			_cloudinary = cloudinary;
			_employer = employer;
		}
		public async Task<ApiResponse<ReviewShowDto>> AddReview(AddReview1 review,IFormFile image,Guid userid)
		{
			try
			{
				string? pic = null;

				if (image != null && image.Length > 0)
				{
					pic = await _cloudinary.UploadImageAsync(image, false);
				}

				var employer = await _employer.GetEmployerByIdAsync(userid);
				if (employer == null)
				{
					return new ApiResponse<ReviewShowDto>(404, "Employer not found");
				}
				var existingReview = await _repository.GetReviewByEmployerAndLabourAsync(employer.EmployerId, review.LabourId);
				if (existingReview != null)
				{
					return new ApiResponse<ReviewShowDto>(400, "You have already reviewed this labourer.");
				}
				Review review1 = new Review
				{
					EmployerId=employer.EmployerId ,
					LabourId=review.LabourId,
					Rating=review.Rating,
					Comment=review.Comment,
					Image=pic
				};
				var res = await _repository.AddReviewAsync(review1);
				if (res == null)
				{
					return new ApiResponse<ReviewShowDto>(400, "Something Went Wrong");
				}


				ReviewShowDto reviewShow = new ReviewShowDto
				{
					Rating=res.Rating,
					Comment=res.Comment,
					Image=res.Image,
					FullName=employer.FullName,
					UpdatedAt=res.UpdatedAt
				};
				if (res != null)
				{
					return new ApiResponse<ReviewShowDto>(200, "Success", reviewShow);
				}
				else
				{
					return new ApiResponse<ReviewShowDto>(400, "Something Went Wrong");
				}
			}catch(Exception ex)
			{
				return new ApiResponse<ReviewShowDto>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<List<ReviewShowDto>>> ShowReviews(Guid Labourid)
		{
			try
			{
				var res = await _repository.GetAllReviews(Labourid);
				var reviewDtos = res.Select(r => new ReviewShowDto
				{
					Rating = r.Rating,
					Comment = r.Comment,
					Image = r.Image,
					FullName = r.Employer?.FullName,
					UpdatedAt = r.UpdatedAt
				}).ToList();
				if (!reviewDtos.Any())
				{
					return new ApiResponse<List<ReviewShowDto>>(404, "NotFound", reviewDtos);
				}
				return new ApiResponse<List<ReviewShowDto>>(200, "Success", reviewDtos);
			}
			catch(Exception ex)
			{
				return new ApiResponse<List<ReviewShowDto>>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<ReviewShowDto>> UpdateReview(Guid reviewId, AddReview1 updatedReview, IFormFile? image, Guid userid)
		{
			try
			{
				var existingReview = await _repository.GetByReviewId(reviewId);	
				Console.WriteLine(existingReview);
				if (existingReview == null)
				{
					return new ApiResponse<ReviewShowDto>(404, "Review not found.");
				}
				var employer = await _employer.GetEmployerByIdAsync(userid);
				if (existingReview.EmployerId != employer.EmployerId)
				{
					return new ApiResponse<ReviewShowDto>(403, "You are not authorized to update this review.");
				}

				if (image != null && image.Length > 0)
				{
					
					if (!string.IsNullOrEmpty(existingReview.Image))
					{
						string oldPublicId = _cloudinary.ExtractPublicIdFromUrl(existingReview.Image);
						await _cloudinary.DeleteImageAsync(oldPublicId);
					}

					existingReview.Image = await _cloudinary.UploadImageAsync(image, false);
				}

				existingReview.Rating = updatedReview.Rating;
				existingReview.Comment = updatedReview.Comment ?? existingReview.Comment;
				existingReview.UpdatedAt = DateTime.UtcNow;
				var updatedRes = await _repository.UpdateAsync(existingReview);

				ReviewShowDto reviewShow = new ReviewShowDto
				{
					Rating = existingReview.Rating,
					Comment = existingReview.Comment,
					Image = existingReview.Image,
					FullName = employer.FullName,
					UpdatedAt = existingReview.UpdatedAt
				};

				return new ApiResponse<ReviewShowDto>(200, "Review updated successfully.", reviewShow);
			}
			catch (Exception ex)
			{
				return new ApiResponse<ReviewShowDto>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<string>> ChangeStatusAsync(Guid reviewid,Guid userid)
		{
			try
			{
				var review = await _repository.GetByReviewIdInActivealso(reviewid);
				if (review == null)
				{
					return new ApiResponse<string>(404, "Review Not Found");
				}
				var employer = await _employer.GetEmployerByIdAsync(userid);
				if (review.EmployerId != employer.EmployerId)
				{
					return new ApiResponse<string>(403, "You are not authorized to change this review status.");
				}
				review.IsActive = !review.IsActive; 
				review.UpdatedAt = DateTime.UtcNow;
				var result = await _repository.UpdateAsync(review);
				if (!result)
				{
					return new ApiResponse<string>(400, "Failed to update review status.");
				}

				return new ApiResponse<string>(200, $"Review status changed to {(review.IsActive ? "Active" : "Inactive")}.");
			}
			catch(Exception ex)
			{
				return new ApiResponse<string>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<List<int>>> GetRating(Guid Labourid)
		{
			try
			{
				var res = await _repository.GetRatingByitsOrder(Labourid);
				if (!res.Any())
				{
					return new ApiResponse<List<int>>(400, "BadRequest", res);
				}
				return new ApiResponse<List<int>>(200, "ok", res);
			}catch(Exception ex)
			{
				return new ApiResponse<List<int>>(500, ex.Message);
			}
		}
	}
}
