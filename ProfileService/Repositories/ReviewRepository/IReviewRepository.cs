using ProfileService.Models;

namespace ProfileService.Repositories.ReviewRepository
{
	public interface IReviewRepository
	{
		Task<Review> AddReviewAsync(Review review);
		Task<List<Review>> GetReviewsByLabour(Guid Labourid);
		Task<Review?> GetReviewByEmployerAndLabourAsync(Guid employerId, Guid labourId);
		Task<Review> GetByReviewId(Guid reviewid);
		Task<bool> UpdateAsync(Review review);
		Task<List<int>> GetRatingByitsOrder(Guid Labourid);
		Task<Review> GetByReviewIdInActivealso(Guid reviewid);
		Task<List<Review>> GetReviewsByEmployee(Guid employeeid);
	}
}
