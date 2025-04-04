﻿using AutoMapper;
//using EventBus.Abstractions;
//using EventBus.Events;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Helper.CloudinaryHelper;
using ProfileService.Helpers.ApiResponse;
using ProfileService.Models;
using ProfileService.Repositories.LabourRepository;
using ProfileService.Services.RabbitMQ;

//using ProfileService.Services.RabbitMQ;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace ProfileService.Services.LabourService
{
    public class LabourService : ILabourService
    {

        private readonly ILabourRepository _labourRepositry;
        private readonly IMapper _mapper;
        private readonly ICloudinaryHelper _cloudinary;
        //private readonly IEventPublisher _eventPublisher;

        private readonly IRabbitMqService _rabbitMqService;
        public LabourService(ILabourRepository labourRepository, IMapper mapper, ICloudinaryHelper cloudinary ,IRabbitMqService rabbitMqService)
        {
            
            _labourRepositry = labourRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _rabbitMqService = rabbitMqService;
            //_eventPublisher = eventPublisher;

        }


        public async Task<string> CompleteLabourProfile(CompleteLabourProfileDto labourProfileDto, Guid userId)

        {
            try
            {

                var isAlreadyLabour = await _labourRepositry.GetLabourByIdAsync(userId);
                if(isAlreadyLabour != null)
                {
                    throw new Exception("Labour already in use ");
                }
                var IsUsedPhone  = await _labourRepositry.GetLabourByPhone(labourProfileDto.PhoneNumber);
                if(IsUsedPhone != null)
                {
                    throw new Exception("Phone number already in use");
                }
                var labour = _mapper.Map<Labour>(labourProfileDto);
                var ImageUrl = await _cloudinary.UploadImageAsync(labourProfileDto.ProfileImage, true);
                labour.UserId = userId;
                labour.ProfilePhotoUrl = ImageUrl;
                var AddLabourResult = await _labourRepositry.AddLabour(labour);
                if (!AddLabourResult)
                {
                    throw new Exception("internal server error : while adding labour details");
                }
                var newLabourId = labour.LabourId;


                foreach (var image in labourProfileDto.LabourWorkImages)
                {

                    var imageUrl = await _cloudinary.UploadImageAsync(image, false);
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        throw new Exception("Failed to upload work image.");
                    }
                    LabourWorkImage workImage = new LabourWorkImage();

                    workImage.LabourId = newLabourId;
                    workImage.ImageUrl = imageUrl;


                    var AddWorkImageResult = await _labourRepositry.AddLabourWorkImages(workImage);

                    if (!AddWorkImageResult)
                    {
                        throw new Exception("internal server error : error occure while adding workimage ");
                    }
                }
                if (labourProfileDto.LabourPreferredMunicipalities != null && labourProfileDto.LabourPreferredMunicipalities.Any())
                {
                    foreach (var item in labourProfileDto.LabourPreferredMunicipalities)
                    {
                        LabourPreferredMuncipality labourPreferredMuncipality = new LabourPreferredMuncipality();
                        labourPreferredMuncipality.LabourId = newLabourId;
                        labourPreferredMuncipality.MunicipalityName = item;
                        if (!await _labourRepositry.AddLabourPreferredMuncipalities(labourPreferredMuncipality))
                        {
                            throw new Exception("failed to add municipality");
                        }
                    }
                }
                if (labourProfileDto.LabourSkills != null && labourProfileDto.LabourSkills.Any())
                {
                    foreach (var item in labourProfileDto.LabourSkills)
                    {
                        LabourSkills labourSkill = new LabourSkills();
                        labourSkill.LabourId = newLabourId;
                        labourSkill.SkillName = item;
                        if (!await _labourRepositry.AddLabourSkills(labourSkill))
                        {
                            throw new Exception("failed to add municipality");
                        }
                    }
                }
                if (await _labourRepositry.UpdateDatabase())
                {

                    _rabbitMqService.PublishProfileCompleted(userId);
                    return "profile Completion successfully completed";

                }
                throw new Exception("internal server erro  : database updation failed");



            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<List<LabourViewDto>> GetFilteredLabour(LabourFilterDto labourFilterDto)
        {
            try
            {
                var allLabours = await _labourRepositry.GetFilterdLabours(labourFilterDto);
                if (allLabours == null || !allLabours.Any())
                {
                    return  new List<LabourViewDto>();
                }
                return _mapper.Map<List<LabourViewDto>>(allLabours);
            }
            catch
            {
                throw;  
            }
        }




       
        public async Task<LabourViewDto> GetLabourById(Guid Id)
        {
            try
            {
                var result = await _labourRepositry.GetLabourByIdAsync(Id);
                if (result == null)
                {
                    return null;
                }

                var labourViewDto = _mapper.Map<LabourViewDto>(result);
                return labourViewDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

       public async Task<LabourViewDto> GetMyDetails(Guid id)
        {
            try
            {
                var result = await _labourRepositry.GetMyDetails(id);
                if (result == null)
                {
                    return null;
                }

                var labourViewDto = _mapper.Map<LabourViewDto>(result);
                return labourViewDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        public async Task<List<LabourViewDto>?> GetAllLabours()
        {
            try
            {
              var allLabours = await _labourRepositry.GetAllLabours();
                if (allLabours == null || !allLabours.Any())
                {
                    return null; 
                }
                
                return _mapper.Map<List<LabourViewDto>>(allLabours);

            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

      
        public async Task<bool> DeleteLabourSkill(Guid userId, string skillName)
        {
            try
            {

                var labour = await _labourRepositry.GetLabourByLabourUserId(userId) ?? throw new Exception("Labour not found");
                var LabourSkill = labour.LabourSkills.FirstOrDefault(skill => skill.SkillName == skillName) ?? throw new Exception("Skill not found");
                labour.LabourSkills.Remove(LabourSkill);
                return await _labourRepositry.UpdateLabour(labour);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in DeleteLabourWorkImages: {ex.Message}", ex);
            }
        }
         public async Task<bool> DeleteLabourMunicipality(Guid userId, string municipalityName) { 
        
            try
            {

                var labour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
                var LabourMuncipality = labour.LabourPreferredMunicipalities.FirstOrDefault(mun=> mun.MunicipalityName == municipalityName) ?? throw new Exception("Muncipality not found");
                labour.LabourPreferredMunicipalities.Remove(LabourMuncipality);
                return await _labourRepositry.UpdateLabour(labour);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in DeleteLabourWorkImages: {ex.Message}", ex);
            }
        }
        
        public async Task<bool> DeleteLabourWorkImages(Guid userId, Guid ImageId)
         {
            try
            {

                var labour = await _labourRepositry.GetLabourByLabourUserId(userId) ?? throw new Exception("Labour not found");
                var LabourWorkImage = labour.LabourWorkImages.FirstOrDefault(image => image.Id == ImageId) ?? throw new Exception("work image not  found");
                labour.LabourWorkImages.Remove(LabourWorkImage);
                return await _labourRepositry.UpdateLabour(labour);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in DeleteLabourWorkImages: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddLabourMunicipality(Guid userId, string municipalityName)
        {
            try
            {

                var labour = await _labourRepositry.GetLabourByLabourUserId(userId) ?? throw new Exception("Labour not found");
                labour.LabourPreferredMunicipalities.Add(new LabourPreferredMuncipality { MunicipalityName = municipalityName });
                return await _labourRepositry.UpdateLabour(labour);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error when AddLabourMuncipality : {ex.Message}", ex);
            }
        }
        
        public async Task<bool> AddLabourSkill(Guid userId, string skillName)

        {
            try
            {

                var labour = await _labourRepositry.GetLabourByLabourUserId(userId) ?? throw new Exception("Labour not found");
                labour.LabourSkills.Add(new LabourSkills { SkillName = skillName });
                return await _labourRepositry.UpdateLabour(labour);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error when AddLabourMuncipality : {ex.Message}", ex);
            }
        }  
        
        public async Task<bool> AddLabourWorkImage(Guid userId, IFormFile image)

        {
            try
            {

                var labour = await _labourRepositry.GetLabourByLabourUserId(userId) ?? throw new Exception("Labour not found");
                if (labour.LabourWorkImages.Count > 3) throw new Exception("please delete one image to add one");
                var imageUrl = await _cloudinary.UploadImageAsync(image, false);
                labour.LabourWorkImages.Add(new LabourWorkImage {ImageUrl = imageUrl });

          
                return await _labourRepositry.UpdateLabour(labour);


            }
            catch (Exception ex)
            {
                throw new Exception($"Error when Adding Labour Work Image : {ex.Message}", ex);
            }
        }

        public async Task<bool> EditLabourProfile (Guid userId , EditLabourProfileDto editLabourProfileDto)
        {
            var existingLabour = await _labourRepositry.GetLabourByLabourUserId(userId) ?? throw new Exception("Labour not found");
            existingLabour.FullName = editLabourProfileDto.FullName ?? existingLabour.FullName;
            existingLabour.AboutYourSelf = editLabourProfileDto.AboutYourSelf ?? existingLabour.AboutYourSelf;
            if (editLabourProfileDto.PreferedTime != null)
            {
                existingLabour.PreferedTime = (Enums.LabourPreferedTime)editLabourProfileDto.PreferedTime;
            }
            if(editLabourProfileDto.Image != null)
            {
                var existingImagePublicId = _cloudinary.ExtractPublicIdFromUrl(existingLabour.ProfilePhotoUrl);
                await _cloudinary.DeleteImageAsync(existingImagePublicId);
                var imageUrl = await _cloudinary.UploadImageAsync(editLabourProfileDto.Image, false);
                existingLabour.ProfilePhotoUrl = imageUrl;
            }
            return await _labourRepositry.UpdateLabour(existingLabour);
        }
        public async Task<ApiResponse<int>> GetLabourCount()
        {
            try
            {
                var res = await _labourRepositry.LabourCountAsync();
                if (res == 0)
                {
                    return new ApiResponse<int>(404, "there is no labours", res);
                }
                return new ApiResponse<int>(200, "success", res);
            }catch(Exception ex)
            {
                return new ApiResponse<int>(500, ex.Message);
            }
        }
    }
}

 