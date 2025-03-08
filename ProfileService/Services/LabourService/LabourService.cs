using AutoMapper;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Dtos;
using ProfileService.Helper.CloudinaryHelper;
using ProfileService.Models;
using ProfileService.Repositories.LabourRepository;
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
        //private readonly IRabbitMqService _rabbitMqService;
        public LabourService(ILabourRepository labourRepository, IMapper mapper, ICloudinaryHelper cloudinary)
        {
            
            _labourRepositry = labourRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
            //_rabbitMqService = rabbitMqService;
            //_eventPublisher = eventPublisher;

        }


        public async Task<LabourProfileCompletionDto> CompleteLabourProfile(CompleteLabourPeofileDto labourPeofileDto, Guid userId)

        {
            try
            {

             var IsUsedPhone  = await _labourRepositry.GetLabourByPhone(labourPeofileDto.LabourProfileCompletionDto.PhoneNumber);

                if(IsUsedPhone != null)
                {
                    throw new Exception("Phone number already in use");
                }


                var labour = _mapper.Map<Labour>(labourPeofileDto.LabourProfileCompletionDto);



                var ImageUrl = await _cloudinary.UploadImageAsync(labourPeofileDto.ProfileImageDto.ImageFile, true);

                labour.UserId = userId;
                labour.ProfilePhotoUrl = ImageUrl;

                var AddLabourResult = await _labourRepositry.AddLabour(labour);
                if (!AddLabourResult)
                {
                    throw new Exception("internal server error : while adding labour details");
                }
                var newLabourId = labour.LabourId;
                foreach (var image in labourPeofileDto.LabourWorkImages)
                {

                    var imageUrl = await _cloudinary.UploadImageAsync(image, false);
                    LabourWorkImage workImage = new LabourWorkImage();

                    workImage.LabourId = newLabourId;
                    workImage.ImageUrl = imageUrl;


                    var AddWorkImageResult = await _labourRepositry.AddLabourWorkImages(workImage);

                    if (!AddWorkImageResult)
                    {
                        throw new Exception("internal server error : error occure while adding workimage ");
                    }
                }
                if (labourPeofileDto.LabourPreferredMunicipalities != null && labourPeofileDto.LabourPreferredMunicipalities.Any())
                {
                    foreach (var item in labourPeofileDto.LabourPreferredMunicipalities)
                    {
                        LabourPreferredMuncipality labourPreferredMuncipality = new LabourPreferredMuncipality();
                        labourPreferredMuncipality.LabourId = newLabourId;
                        labourPreferredMuncipality.MunicipalityId = item;
                        if (!await _labourRepositry.AddLabourPreferredMuncipalities(labourPreferredMuncipality))
                        {
                            throw new Exception("failed to add municipality");
                        }
                    }
                }
                if (labourPeofileDto.LabourSkills != null && labourPeofileDto.LabourSkills.Any())
                {
                    foreach (var item in labourPeofileDto.LabourSkills)
                    {
                        LabourSkills labourSkill = new LabourSkills();
                        labourSkill.LabourId = newLabourId;
                        labourSkill.SkillId = item;
                        if (!await _labourRepositry.AddLabourSkills(labourSkill))
                        {
                            throw new Exception("failed to add municipality");
                        }
                    }
                }
                if (await _labourRepositry.UpdateDatabase())
                {

                    //_rabbitMqService.PublishProfileCompleted(userId);
                    //_eventPublisher.Publish(new ProfileCompletedEvent { UserId = userId });
                    return labourPeofileDto.LabourProfileCompletionDto;
                }
                throw new Exception("internal server erro  : database updation failed");



            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }



        public async Task<List<LabourViewDto>> GetFilteredLabour(LabourFilterDto LabourFilterDto)
        {
            

            var allLabours = await _labourRepositry.GetFilterdLabours(LabourFilterDto);
            if(allLabours == null)
            {
                return null; 
            }
            var labourViewDtos = allLabours.Select(result => new LabourViewDto
            {
                LabourId = result.LabourId,
                LabourProfileCompletion = _mapper.Map<LabourProfileCompletionDto>(result),
                ProfilePhotoUrl = result.ProfilePhotoUrl,
                LabourWorkImages = result.LabourWorkImages.Select(img => img.ImageUrl).ToList(),
                LabourPreferredMuncipalities = result.LabourPreferedMuncipalities.Select(m => m.MunicipalityId).ToList(),
                LabourSkills = result.LabourSkills.Select(s => s.SkillId).ToList()
            }).ToList();

            return labourViewDtos;



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

                var labourViewDto = new LabourViewDto
                {
                    LabourId = result.LabourId,
                    LabourProfileCompletion = _mapper.Map<LabourProfileCompletionDto>(result),
                    ProfilePhotoUrl = result.ProfilePhotoUrl,
                    LabourWorkImages = result.LabourWorkImages.Select(img => img.ImageUrl).ToList(),
                    LabourPreferredMuncipalities = result.LabourPreferedMuncipalities.Select(m => m.MunicipalityId).ToList(),
                    LabourSkills = result.LabourSkills.Select(s => s.SkillId).ToList()
                };

                return labourViewDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message }", ex);
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
                var labourViewDtos = allLabours.Select(result => new LabourViewDto
                {
                    LabourId = result.LabourId,
                    LabourProfileCompletion = _mapper.Map<LabourProfileCompletionDto>(result),
                    ProfilePhotoUrl = result.ProfilePhotoUrl,
                    LabourWorkImages = result.LabourWorkImages.Select(img => img.ImageUrl).ToList(),
                    LabourPreferredMuncipalities = result.LabourPreferedMuncipalities.Select(m => m.MunicipalityId).ToList(),
                    LabourSkills = result.LabourSkills.Select(s => s.SkillId).ToList()
                }).ToList();

                return labourViewDtos;

            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.InnerException?.Message ?? ex.Message}", ex);
            }
        }

        //public async Task<LabourProfileCompletionDto> UpdatLabourProfile(CompleteLabourPeofileDto labourProfileDto, Guid Id)
        //{
        //    try
        //    {
        //        var existigLabour = _labourRepositry.GetLabourByIdAsync(Id);
        //        if (existigLabour == null)
        //        {
        //            return null;
        //        }




        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public async Task<bool> DeleteLabourSkill(Guid userId, Guid SkillId)
        {
            try
            {

                var labour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
                var LabourSkill = labour.LabourSkills.FirstOrDefault(skill => skill.SkillId == SkillId) ?? throw new Exception("Skill not found");
                labour.LabourSkills.Remove(LabourSkill);
                return await _labourRepositry.UpdateLabour(labour);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in DeleteLabourWorkImages: {ex.Message}", ex);
            }
        }
         public async Task<bool> DeleteLabourMunicipality(Guid userId, int id)
        {
            try
            {

                var labour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
                var LabourMuncipality = labour.LabourPreferedMuncipalities.FirstOrDefault(mun=> mun.MunicipalityId == id) ?? throw new Exception("Muncipality not found");
                labour.LabourPreferedMuncipalities.Remove(LabourMuncipality);
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

                var labour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
                var LabourWorkImage = labour.LabourWorkImages.FirstOrDefault(image => image.Id == ImageId) ?? throw new Exception("work image not  found");
                labour.LabourWorkImages.Remove(LabourWorkImage);
                return await _labourRepositry.UpdateLabour(labour);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error in DeleteLabourWorkImages: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddLabourMunicipality(Guid userId, int municipalityId)
        {
            try
            {

                var labour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
                labour.LabourPreferedMuncipalities.Add(new LabourPreferredMuncipality { MunicipalityId = municipalityId });
                return await _labourRepositry.UpdateLabour(labour);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error when AddLabourMuncipality : {ex.Message}", ex);
            }
        }
        
        public async Task<bool> AddLabourSkill(Guid userId, Guid SkillId)

        {
            try
            {

                var labour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
                labour.LabourSkills.Add(new LabourSkills { SkillId = SkillId });
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

                var labour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
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
            var existingLabour = await _labourRepositry.GetLabourByIdAsync(userId) ?? throw new Exception("Labour not found");
            existingLabour.FullName = editLabourProfileDto.FullName ?? existingLabour.FullName;
            //existingLabour.PreferedTime = editLabourProfileDto.LabourProfileCompletionDto.PreferedTime ?? existingLabour.PreferedTime;
            //existingLabour.ProfilePhotoUrl = editLabourProfileDto.ProfileImageDto.ImageFile
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

    }
}

 