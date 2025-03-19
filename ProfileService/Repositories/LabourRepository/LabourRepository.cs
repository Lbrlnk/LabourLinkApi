using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Dtos;
using ProfileService.Models;

namespace ProfileService.Repositories.LabourRepository
{
    public class LabourRepository : ILabourRepository
    {

        private readonly LabourLinkProfileDbContext _context;
        public LabourRepository(LabourLinkProfileDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddLabour(Labour lbr)
        {
            await _context.Labours.AddAsync(lbr);
            return true;
        }


        public async Task<bool> AddLabourPreferredMuncipalities(LabourPreferredMuncipality lpm)
        {
            await _context.LabourPreferedMuncipalities.AddAsync(lpm);
            return true;

        }
        public async Task<bool> AddLabourWorkImages(LabourWorkImage lwi)
        {
            Console.WriteLine("labour workimage " , lwi);
            await _context.LabourWorkImages.AddAsync(lwi);
            return true;
        }

        public async Task<bool> AddLabourSkills(LabourSkills skill)
        {
            await _context.LabourSkills.AddAsync(skill);
            return true;
        }

        public async Task<bool> UpdateDatabase()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Labour> GetLabourByIdAsync(Guid Id)
        {
            return await _context.Labours
                .Include(l => l.LabourSkills)
                .Include(l => l.LabourWorkImages)
                .Include(l => l.LabourPreferredMunicipalities)
                .Include(l => l.Reviews)
                .ThenInclude(r => r.Employer)
                .FirstOrDefaultAsync(s => s.LabourId == Id);
        }

        public async Task<List<Review>> GetLabourReviews(Guid labourId)
        {
            return await _context.Reviews.Where(l => l.LabourId == labourId).ToListAsync();
        }


        public async Task<List<LabourWorkImage>> GetLabourWorkImages(Guid Id)
        {
            return await _context.LabourWorkImages.Where(img => img.LabourId == Id).ToListAsync();
        }
        public async Task<List<LabourSkills>> GetLabourSkills(Guid Id)
        {
            return await _context.LabourSkills.Where(skill => skill.LabourId == Id).ToListAsync();
        }
        public async Task<List<LabourPreferredMuncipality>> GetLabourMuncipalities(Guid Id)
        {
            return await _context.LabourPreferedMuncipalities.Where(mun => mun.LabourId == Id).ToListAsync();
        }

        public async Task<List<Labour>> GetAllLabours() 
        {
            return await _context.Labours
                 .Include(l => l.LabourSkills)
                 .Include(l => l.LabourWorkImages)
                 .Include(l => l.LabourPreferredMunicipalities) 
                 .Where(l => l.IsActive == true)
                 .ToListAsync();
        }



        public async Task<List<Labour>> GetFilterdLabours(LabourFilterDto filterDto)
        {
            var query = _context.Labours
                .Include(l => l.LabourSkills)
                .Include(l => l.LabourWorkImages)
                .Include(l => l.LabourPreferredMunicipalities)
                .Where(l => l.IsActive)
                .AsNoTracking();  

            if (filterDto.PreferredMunicipalities != null && filterDto.PreferredMunicipalities.Any())
            {
                query = query.Where(l => l.LabourPreferredMunicipalities
                    .Any(m => filterDto.PreferredMunicipalities.Contains(m.MunicipalityName)));
            }

            if (filterDto.Skills != null && filterDto.Skills.Any())
            {
                query = query.Where(l => l.LabourSkills
                    .Any(s => filterDto.Skills.Contains(s.SkillName)));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateLabour(Labour labour)
        {
            _context.Labours.Update(labour);
          return  await  _context.SaveChangesAsync() > 0;
        }

        public async Task<Labour> GetLabourByPhone(string phoneNumber)
        {   
           return await _context.Labours.FirstOrDefaultAsync(l => l.PhoneNumber == phoneNumber); 

        }

		public async Task<Labour> GetLabourByuserIdAsync(Guid UserId)
		{
			return await _context.Labours
				.Include(l => l.LabourSkills)
				.Include(l => l.LabourWorkImages)
				.Include(l => l.LabourPreferredMunicipalities)
				.Include(l => l.Reviews)
				.ThenInclude(r => r.Employer)
				.FirstOrDefaultAsync(s => s.UserId == UserId);
		}
        public async Task<int> LabourCountAsync()
        {
            return await _context.Labours.CountAsync();
        }
	


        public async Task<Labour> GetMyDetails(Guid id)
        {
            return await _context.Labours
                .Include(l => l.LabourSkills)
                .Include(l => l.LabourWorkImages)
                .Include(l => l.LabourPreferredMunicipalities)
                .Include(l => l.Reviews)
                .ThenInclude(r => r.Employer)
                .FirstOrDefaultAsync(s => s.UserId == id);
        }
    }

}
