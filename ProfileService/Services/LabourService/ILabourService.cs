using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Helpers.ApiResponse;
using ProfileService.Models;

namespace ProfileService.Services.LabourService
{
    public interface ILabourService
    {
        //Task<Labour> CompleteLabourProfile(LabourProfileCompletionDto lbrProfileCompletionDto, ProfileImageDto profileImageDto, LabourWorkImageDto lbrWrkImgDto , Guid userId);
        Task<string> CompleteLabourProfile(CompleteLabourProfileDto labourPeofileDto, Guid userId);
        Task<LabourViewDto> GetLabourById(Guid Id);


        Task<LabourViewDto> GetMyDetails(Guid id);
        


        Task<bool> DeleteLabourSkill(Guid userId, string skillname);
        Task<bool> DeleteLabourMunicipality(Guid userId, string muncipalityName);
        Task<bool> DeleteLabourWorkImages(Guid userId, Guid imageId);

        Task<bool> AddLabourMunicipality(Guid userId, string municipalityName);
        Task<bool> AddLabourWorkImage(Guid userId, IFormFile image);

        Task<bool> AddLabourSkill(Guid userId, string skillName);
        Task<bool> EditLabourProfile(Guid userId, EditLabourProfileDto editLabourProfileDto);

        Task<List<LabourViewDto>> GetAllLabours();

        Task<List<LabourViewDto>> GetFilteredLabour(LabourFilterDto LabourFilterDto);


        Task<ApiResponse<int>> GetLabourCount();




	}
}
