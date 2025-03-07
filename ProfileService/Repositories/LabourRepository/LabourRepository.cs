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
                .Include(l => l.LabourPreferedMuncipalities)
                .FirstOrDefaultAsync(s => s.UserId == Id);
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
                 .Include(l => l.LabourPreferedMuncipalities)
                 .Where(l => l.IsActive == true)
                 .ToListAsync();
        }

        public async Task<List<Labour>> GetFilterdLabours(LabourFilterDto filterDto)
        {
            var query = _context.Labours
        .Include(l => l.LabourSkills)
        .Include(l => l.LabourWorkImages)
        .Include(l => l.LabourPreferedMuncipalities)
        .Where(l => l.IsActive == true)
        .AsQueryable(); 

            
            if (filterDto.PreferredMunicipalities != null && filterDto.PreferredMunicipalities.Any())
            {
                query = query.Where(l => l.LabourPreferedMuncipalities
                    .Any(m => filterDto.PreferredMunicipalities.Contains(m.MunicipalityId)));
            }

            if (filterDto.SkillIds != null && filterDto.SkillIds.Any())
            {
                query = query.Where(l => l.LabourSkills
                    .Any(s => filterDto.SkillIds.Contains(s.SkillId)));
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
    }
}
