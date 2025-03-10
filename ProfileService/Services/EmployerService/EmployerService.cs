using AutoMapper;

using EventBus.Abstractions;
using EventBus.Events;
using ProfileService.Dtos;
using ProfileService.Helper.CloudinaryHelper;
using ProfileService.Models;
using ProfileService.Repositories.EmployerRepository;
using ProfileService.Repositories.LabourRepository;
//using ProfileService.Services.RabbitMQ;


namespace ProfileService.Services.EmployerService
{
    public class EmployerService : IEmployerService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEmployerRepository _employerRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryHelper _cloudinary;
        //private readonly IRabbitMqService _rabbitMqService;

        public EmployerService(IEmployerRepository employerRepository, IMapper mapper, IEventPublisher eventPublisher, ICloudinaryHelper cloudinary)
        {
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _employerRepository = employerRepository;
            _cloudinary = cloudinary;
            //_rabbitMqService = rabbitMqService;

        }
        public async Task<string> CompleteEmployerProfile(Guid userId, CompleteEmployerProfileDto employerProfileDto)
        {
            try
            {
                var employer = _mapper.Map<Employer>(employerProfileDto);

                if(employerProfileDto.ProfileImage != null)
                {
                    var ImageUrl = await _cloudinary.UploadImageAsync(employerProfileDto.ProfileImage, true);
                    employer.ProfileImageUrl = ImageUrl;
                }

                 employer.UserId = userId;
                 await _employerRepository.AddEmployer(employer);
                if (await _employerRepository.UpdateDatabase())
                {
                    //_rabbitMqService.PublishProfileCompleted(userId);

                    _eventPublisher.Publish(new ProfileCompletedEvent { UserId = userId });
                    return "registration succesfull";


                }

                throw new Exception("internal server erro  : database updation failed");
                
            }
            catch (Exception ex)
            {
                throw;
            }

        }

       public async Task<string> UpdateEmployerProfile(Guid userId, EditEmployerProfileDto employerProfileDto)
        {
            try
            {
                var existingEmployer = await  _employerRepository.GetEmployerByIdAsync(userId) ?? throw new Exception("Labour not found");


                existingEmployer.FullName = employerProfileDto.FullName??existingEmployer.FullName;

                if (employerProfileDto.Image != null)
                {
                    var existingImagePublicId = _cloudinary.ExtractPublicIdFromUrl(existingEmployer.ProfileImageUrl);
                    await _cloudinary.DeleteImageAsync(existingImagePublicId);
                    var imageUrl = await _cloudinary.UploadImageAsync(employerProfileDto.Image, false);
                    existingEmployer.ProfileImageUrl = imageUrl;
                }

                existingEmployer.PreferedMunicipality = employerProfileDto.PreferedMuncipality ?? existingEmployer.PreferedMunicipality;


              
               await _employerRepository.UpdateEmployer(existingEmployer);
                
                
                return "Updation successfull";


            }
            catch (Exception ex)
            {
                throw new Exception($"Error when Updating Employer Profile {ex.Message}", ex);
            }
        }
       public async  Task<EmployerView> GetEmployerDetails(Guid userId)
        {
            try
            {
                var result = await _employerRepository.GetEmployerByIdAsync(userId);
                if (result is null)
                {
                    return null;
                }

                var employerView = _mapper.Map<EmployerView>(result);
                return employerView;

            } catch (Exception ex)
            {
                throw new Exception($"Error when retriving Employer  : {ex.Message}", ex);
            }
        }
    }
}
