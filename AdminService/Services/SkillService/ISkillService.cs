using AdminService.Dtos.SkillDtos;
using AdminService.Helpers.Common;

namespace AdminService.Services
{
    public interface ISkillService
    {

        Task<ApiResponse<IEnumerable<SkillViewDto>>> GetAllSkillAsync();

        Task<ApiResponse<SkillViewDto>> GetSkillByIdAsync(Guid skillId);

        Task<ApiResponse<SkillViewDto>> CreateSkillAsync(AddSkillDto newSkill);

        Task<ApiResponse<SkillViewDto>> UpdateAsync(SkillViewDto UpdateSkill);

        Task<ApiResponse<string>> DeleteAsync(Guid skillId);

        Task<ApiResponse<IEnumerable<SkillViewDto>>> GetAllDeletedSkillAsync();

        Task <ApiResponse<SkillViewDto>> SkillReactivationAsync(Guid skillId);

        Task<ApiResponse<IEnumerable<SkillViewDto>>> GetSkillbySearchParams(string searchParams);

        // this service is for admin to get every skill,the active and deactive
        Task<ApiResponse<List<AdminViewSkillDto>>> GetCompleteSkills();


    }
}
