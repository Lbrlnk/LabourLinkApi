using AdminService.Dtos.SkillDtos;
using AdminService.Models;

namespace AdminService.Repository.SkillRepository
{
    public interface ISkillRepostory
    {

        Task<IEnumerable<Skill>> GetAllSkillsAsync();

        Task<Skill> GetSkillByIdAsync(Guid id);
        Task<Skill> CreateSkillAsync(AddSkillDto newskill);

        Task<bool> UpdateSkillAsync(Skill updateSkill);

       


    }
}
