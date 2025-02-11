using AdminService.Data;
using AdminService.Dtos.SkillDtos;
using AdminService.Helpers.Common;
using AdminService.Repository.SkillRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace AdminService.Services
{
    public class SkillService:ISkillService
    {
       
        private readonly IMapper _mapper;
        private readonly ISkillRepostory _skillRepostory;
        
        public SkillService(AdminDbContext context,IMapper mapper,ISkillRepostory skillRepository)
        {
           
            _mapper = mapper;
            _skillRepostory = skillRepository;
        }

        public async Task<ApiResponse<IEnumerable<SkillViewDto>>> GetAllSkillAsync()
        {
            try
            {
                var allSkill = await _skillRepostory.GetSkillsAsync();

                if (!allSkill.Any())
                {
                    return new ApiResponse<IEnumerable<SkillViewDto>>(200, "No Skill Found", new List<SkillViewDto>());
                }

                var result = _mapper.Map<List<SkillViewDto>>(allSkill);

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

        public async Task<ApiResponse<IEnumerable<SkillViewDto>>> GetAllDeletedSkillAsync()
        {
            try
            {
                var deletedSkills = await _skillRepostory.GetAllDeletedSkillAsync();

                var response=_mapper.Map<List<SkillViewDto>>(deletedSkills);
                return new ApiResponse<IEnumerable<SkillViewDto>>(200, "Successfully retrieved deleted skills",response);
            }
            catch (Exception ex)
            {

                return new ApiResponse<IEnumerable<SkillViewDto>>(500, "Error occurred while retrieving skills", error: ex.Message);
            }
        }

        public async Task<ApiResponse<SkillViewDto>> SkillReactivationAsync(Guid skillId)
        {
            try
            {
                var deletedSkill = await _skillRepostory.GetDeletedSkillByIdAsync(skillId);

                if (deletedSkill == null)
                {
                    return new ApiResponse<SkillViewDto>(404, "Skill not found");
                }

                deletedSkill.IsActive = true;

                var activateSkill = await _skillRepostory.UpdateSkillAsync(deletedSkill);

                if(!activateSkill)
                {
                    return new ApiResponse<SkillViewDto>(500, "Skill Reactivation failled");
                }

                var response=_mapper.Map<SkillViewDto>(deletedSkill);

                return new ApiResponse<SkillViewDto>(200,"Successfully Activated the Skill",response);


                
            }
            catch (Exception ex)
            {

                return new ApiResponse<SkillViewDto>(500, "Error occurred while retrieving skills", error: ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<SkillViewDto>>>GetSkillbySearchParams(string searchParams)
        {
            try
            {

                var searchedSkills=await _skillRepostory.GetSkillBySearchParamsAsync(searchParams);

                if (searchedSkills == null)
                {
                    var emptyskill = new List<SkillViewDto>();
                    return new ApiResponse<IEnumerable<SkillViewDto>>(200, "No skill Found", emptyskill);
                }

                var response = _mapper.Map<List<SkillViewDto>>(searchedSkills);

                return new ApiResponse<IEnumerable<SkillViewDto>>(200, "No skill Found", response);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<SkillViewDto>>(500, "Error occurred while retrieving skills", error: ex.Message);
            }

        }

        public async Task<ApiResponse<List<AdminViewSkillDto>>> GetCompleteSkills()

        {
            try
            {
                var allSkill = await _skillRepostory.GetAllSkillsAsync();

                var response = _mapper.Map<List<AdminViewSkillDto>>(allSkill);

                return new ApiResponse<List<AdminViewSkillDto>>(200, "Successfully retrived Complete Skills", response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AdminViewSkillDto>>(500, "Error occurred while retrieving skills", error: ex.Message);
            }
        }
    }
}
