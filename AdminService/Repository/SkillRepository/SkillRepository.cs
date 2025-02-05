using AdminService.Data;
using AdminService.Dtos.SkillDtos;
using AdminService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Repository.SkillRepository
{
    public class SkillRepository:ISkillRepostory
    {
        private readonly AdminDbContext _context; 
        public SkillRepository(AdminDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            return await _context.Skills.ToListAsync();
        }

        

        public async Task<Skill> GetSkillByIdAsync(Guid id)
        {
            return await _context.Skills.FirstOrDefaultAsync(x => x.SkillId == id);
        }


        public async Task<Skill> CreateSkillAsync(AddSkillDto newskill)
        {
            
            var newSkill=new Skill();
            newSkill.SkillName=newskill.Name;
            await _context.AddAsync(newSkill);
            await _context.SaveChangesAsync();
            return newSkill;


        }

        public async Task<bool> UpdateSkillAsync(Skill updateSkill)
        {
              _context.Skills.Update(updateSkill);

            return await _context.SaveChangesAsync() > 0;
        }

       
    }
}
