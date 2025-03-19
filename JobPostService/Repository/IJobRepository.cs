using JobPostService.Models;

namespace JobPostService.Repository
{
	public interface IJobRepository
	{
		Task<JobPost> AddJobPost(JobPost jobpost);
		Task<List<JobPost>> GetJobPostsAsync();
		Task<List<JobPost>> GetPostAsyncActiveAsync();
		Task<bool> UpdatePostAsync(JobPost jobPost);
		Task<JobPost> GetJobPostByIdAsync(Guid id);
		Task<List<JobPost>> GetJobPostByClientAsync(Guid clientid);
		Task<List<JobPost>> GetJobPostBySearchParamsAsync(string searchParams);
		Task<List<JobPost>> GetJobPostBySkillandMuncipalityAsync(string municipality, List<string> skills);
	}
}
