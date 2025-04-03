using JobPostService.Data;
using JobPostService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace JobPostService.Repository
{
	public class JobRepository: IJobRepository
	{
		private readonly AppDbContext _context;
		public JobRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task<JobPost> AddJobPost(JobPost jobpost)
		{
			await _context.JobPosts.AddAsync(jobpost);
			await _context.SaveChangesAsync();
			return jobpost;
		}
		public async Task<List<JobPost>> GetJobPostsAsync()
		{
			return await _context.JobPosts.OrderByDescending(x=>x.CreatedDate).ToListAsync();
		}
		public async Task<List<JobPost>> GetPostAsyncActiveAsync()
		{
			return await _context.JobPosts.Where(x=>x.Status== "Active" && x.EndDate >= DateOnly.FromDateTime(DateTime.Today)).OrderByDescending(x => x.CreatedDate).ToListAsync();
		}
		public async Task<bool> UpdatePostAsync(JobPost jobPost)
		{
			_context.JobPosts.Update(jobPost);
			return await _context.SaveChangesAsync()>0;
		}
		public async Task<JobPost>GetJobPostByIdAsync(Guid id)
		{
			return await _context.JobPosts.FirstOrDefaultAsync(x => x.JobId == id && x.Status=="Active" && x.EndDate >= DateOnly.FromDateTime(DateTime.Today));
		}
		public async Task<List<JobPost>> GetJobPostByClientAsync(Guid clientid)
		{
			return await _context.JobPosts.Where(x=>x.CleintId==clientid).OrderByDescending(x => x.CreatedDate).ToListAsync();
		}
		public async Task<List<JobPost>> GetJobPostBySearchParamsAsync(string searchParams)
		{
			return await _context.JobPosts.Where(x => x.Title.ToLower().Contains(searchParams.ToLower()) && x.Status == "Active" && x.EndDate >= DateOnly.FromDateTime(DateTime.Today)).OrderBy(x => x.Title).ToListAsync();
		}
		public async Task<List<JobPost>> GetJobPostBySkillandMuncipalityAsync(string municipality, List<string> skills)
		{
			return await _context.JobPosts
					.Where(job => job.MuncipalityId == municipality ||
					 (skills.Contains(job.SkillId1) || skills.Contains(job.SkillId2)))
					.ToListAsync();
		}

		
	}
}
