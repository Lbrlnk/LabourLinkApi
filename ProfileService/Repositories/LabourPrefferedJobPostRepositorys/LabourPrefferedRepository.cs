using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Models;

namespace ProfileService.Repositories.LabourPrefferedRepositorys
{
	public class LabourPrefferedRepository : ILabourPrefferedRepository
	{
		private readonly LabourLinkProfileDbContext _context;
		public LabourPrefferedRepository(LabourLinkProfileDbContext context)
		{
			_context = context;
		}

		public async Task<List<JobPost>> GetMatchingJobPostsAsync(Guid labourId)
		{
			// Get preferred municipalities and skills of labour
			var labour = await _context.Labours
				.Include(l => l.LabourPreferedMuncipalities)
				.Include(l => l.LabourSkills)
				.Where(l => l.UserId == labourId)
				.FirstOrDefaultAsync();

			if (labour == null)
			{
				return new List<JobPost>();
			}

			var preferredMunicipalities = labour.LabourPreferedMuncipalities
				.Select(pm => pm.MunicipalityId)
				.ToList();

			var labourSkills = labour.LabourSkills
				.Select(ls => ls.SkillId)
				.ToList();

			var matchingJobPosts = await _context.JobPost
				.Where(jp => preferredMunicipalities.Contains(jp.MuncipalityId) &&
							(labourSkills.Contains(jp.SkillId1) ||
							(jp.SkillId2.HasValue && labourSkills.Contains(jp.SkillId2.Value))))
				.ToListAsync();

			return matchingJobPosts;
		}
	}
}
