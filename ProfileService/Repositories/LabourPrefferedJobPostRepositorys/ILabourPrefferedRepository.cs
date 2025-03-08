using ProfileService.Models;

namespace ProfileService.Repositories.LabourPrefferedRepositorys
{
	public interface ILabourPrefferedRepository
	{
		Task<List<JobPost>> GetMatchingJobPostsAsync(Guid labourId);
	}
}
