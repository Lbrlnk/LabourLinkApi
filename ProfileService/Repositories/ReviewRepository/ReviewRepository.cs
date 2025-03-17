using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Models;

namespace ProfileService.Repositories.ReviewRepository
{
	public class ReviewRepository: IReviewRepository
	{
		private readonly LabourLinkProfileDbContext _context;
		public ReviewRepository(LabourLinkProfileDbContext context)
		{
			_context = context;
		}
		public async Task<Review> AddReviewAsync(Review review)
		{
			await _context.Reviews.AddAsync(review);
			await _context.SaveChangesAsync();
			return review;
		}
		public async Task<List<Review>> GetReviewsByLabour(Guid Labourid)
		{
			return await _context.Reviews
				.Where(r => r.LabourId == Labourid && r.IsActive == true)
				.Include(r => r.Employer) 
				.ToListAsync();
		}
		public async Task<Review?> GetReviewByEmployerAndLabourAsync(Guid employerId, Guid labourId)
		{
			return await _context.Reviews
				.FirstOrDefaultAsync(x => x.EmployerId == employerId && x.LabourId == labourId && x.IsActive == true);
		}
		public async Task<Review> GetByReviewId(Guid reviewid)
		{
			return await _context.Reviews.FirstOrDefaultAsync(x => x.ReviewId == reviewid && x.IsActive == true);
		}
		public async Task<Review> GetByReviewIdInActivealso(Guid reviewid)
		{
			return await _context.Reviews.FirstOrDefaultAsync(x => x.ReviewId == reviewid);
		}
		public async Task<bool> UpdateAsync(Review review)
		{
			_context.Reviews.Update(review);
			return await _context.SaveChangesAsync()>0;
		}
		public async Task<List<int>> GetRatingByitsOrder(Guid Labourid)
		{
			return new List<int>
			{
				await _context.Reviews.CountAsync(x => x.LabourId == Labourid && x.Rating == 1 && x.IsActive==true),
				await _context.Reviews.CountAsync(x => x.LabourId == Labourid && x.Rating == 2 && x.IsActive==true),
				await _context.Reviews.CountAsync(x => x.LabourId == Labourid && x.Rating == 3 && x.IsActive == true),
				await _context.Reviews.CountAsync(x => x.LabourId == Labourid && x.Rating == 4 && x.IsActive == true),
				await _context.Reviews.CountAsync(x => x.LabourId == Labourid && x.Rating == 5 && x.IsActive == true)
			};
		}
		public async Task<List<Review>> GetReviewsByEmployee(Guid employeeid)
		{
			return await _context.Reviews
				.Where(r => r.EmployerId == employeeid )
				.Include(r => r.Labour)
				.ToListAsync();
		}
	}
}
