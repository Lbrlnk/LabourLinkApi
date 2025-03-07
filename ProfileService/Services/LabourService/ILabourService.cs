using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Models;

namespace ProfileService.Services.LabourService
{
    public interface ILabourService
    {
        //Task<Labour> CompleteLabourProfile(LabourProfileCompletionDto lbrProfileCompletionDto, ProfileImageDto profileImageDto, LabourWorkImageDto lbrWrkImgDto , Guid userId);
        Task<LabourProfileCompletionDto> CompleteLabourProfile(CompleteLabourPeofileDto labourPeofileDto, Guid userId);
        Task<LabourViewDto> GetLabourById(Guid Id);

        //Task<LabourProfileCompletionDto> UpdatLabourProfile(CompleteLabourPeofileDto labourProfileDto , Guid Id);


        Task<bool> DeleteLabourSkill(Guid userId, Guid skillId);
        Task<bool> DeleteLabourMunicipality(Guid userId, int id);
        Task<bool> DeleteLabourWorkImages(Guid userId, Guid imageId);

        Task<bool> AddLabourMunicipality(Guid userId, int municipalityId);
        Task<bool> AddLabourWorkImage(Guid userId, IFormFile image);

        Task<bool> AddLabourSkill(Guid userId, Guid skillId);
        Task<bool> EditLabourProfile(Guid userId, EditLabourProfileDto editLabourProfileDto);







    }
}
