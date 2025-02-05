using AdminService.Data;
using AdminService.Dtos.SkillDtos;
using AdminService.Helpers.Common;
using AdminService.Repository.SkillRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AdminService.Services
{
    public class SkillService:ISkillService
    {
        private readonly AdminDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISkillRepostory _skillRepostory;
        
        public SkillService(AdminDbContext context,IMapper mapper,ISkillRepostory skillRepository)
        {
            _context = context;
            _mapper = mapper;
            _skillRepostory = skillRepository;
        }

        public async Task<ApiResponse<IEnumerable<SkillViewDto>>> GetAllSkillAsync()
        {
            try
            {
                var allSkill = await _skillRepostory.GetAllSkillsAsync();

                if (!allSkill.Any())
                {
                    return new ApiResponse<IEnumerable<SkillViewDto>>(200, "No Skill Found", new List<SkillViewDto>());
                }

                var result = _mapper.Map<IEnumerable<SkillViewDto>>(allSkill);

                return new ApiResponse<IEnumerable<SkillViewDto>>(200, "Skills retrieved successfully", result);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SkillViewDto>>(500, "Error occurred while retrieving skills",error:ex.Message);

            }
        }

        public async Task<ApiResponse<SkillViewDto>> GetSkillByIdAsync(Guid skillId)
        {
            try
            {
                var skill =await _skillRepostory.GetSkillByIdAsync(skillId);

                if(skill == null)
                {
                    return new ApiResponse<SkillViewDto>(400, "Mo Skill Found");
                }

                var result= _mapper.Map<SkillViewDto>(skill);

                return new ApiResponse<SkillViewDto>(200, "Skill retrieved successfully",result);


            }
            catch (Exception ex)
            {
                return new ApiResponse<SkillViewDto>(500, "Error occurred while retrieving skills", error: ex.Message);
            }
        }

       public async Task<ApiResponse<SkillViewDto>> CreateSkillAsync(AddSkillDto newSkill)
        {
            try
            {
                var result = await _skillRepostory.CreateSkillAsync(newSkill);

                var res=_mapper.Map<SkillViewDto>(result);

                return new ApiResponse<SkillViewDto>(201, "Successfully Added new Skill", res);

            }
            catch (Exception ex)
            {
                return new ApiResponse<SkillViewDto>(500, "Error occurred while retrieving skills", error: ex.Message);
            }
        }

        public async Task<ApiResponse<SkillViewDto>> UpdateAsync(SkillViewDto UpdateSkill)
        {
            try
            {
                var modifySkill = await _skillRepostory.GetSkillByIdAsync(UpdateSkill.SkillId);
                if (modifySkill == null)
                {
                    return new ApiResponse<SkillViewDto>(404, "Not Found");
                }

                modifySkill.SkillName = UpdateSkill.SkillName;
                modifySkill.UpdatedAt = DateTime.UtcNow;

                var checkUpdate = await _skillRepostory.UpdateSkillAsync(modifySkill);

                if (!checkUpdate)
                {
                    return new ApiResponse<SkillViewDto>(500, "Updation failed");
                }

                var result=_mapper.Map<SkillViewDto>(modifySkill);

                return new ApiResponse<SkillViewDto>(200,"Updated Successfully",result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<SkillViewDto>(500, "Error occurred while retrieving skills", error: ex.Message);
            }


        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid skillId)
        {

            try
            {
                var isSkillExist =await  _skillRepostory.GetSkillByIdAsync(skillId);

                if(isSkillExist != null)
                {
                    isSkillExist.IsActive = false;

                    var result = await _skillRepostory.UpdateSkillAsync(isSkillExist);
                    if (!result)
                    {
                        return new ApiResponse<string>(500, "Deletion failed", error: "Deletetion failde due error on database ");
                    }

                    return new ApiResponse<string>(200,"Deleted Successfully",isSkillExist.SkillName);
                }
                return new ApiResponse<string>(400, "No Skill Found",error:"Invalid Skill Id");


            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, "Error occurred while retrieving skills", error: ex.Message);
            }
        }


    }
}
