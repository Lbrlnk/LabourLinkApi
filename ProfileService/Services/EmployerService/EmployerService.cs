using AutoMapper;
using EventBus.Abstractions;
using EventBus.Events;
using ProfileService.Dtos;
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
        //private readonly IRabbitMqService _rabbitMqService;

        public EmployerService(IEmployerRepository employerRepository, IMapper mapper, IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _employerRepository = employerRepository;
            //_rabbitMqService = rabbitMqService;

        }
        public async Task<CompleteEmployerProfileDto> CompleteEmployerProfile(Guid userId, CompleteEmployerProfileDto employerProfileDto)
        {
            try
            {
                var employee = _mapper.Map<Employer>(employerProfileDto);

                employee.UserId = userId;
                 await _employerRepository.AddEmployer(employee);
                if(await _employerRepository.UpdateDatabase())
                {
                    //_rabbitMqService.PublishProfileCompleted(userId);
                    _eventPublisher.Publish(new ProfileCompletedEvent { UserId = userId });
                    return employerProfileDto;

                }

                throw new Exception("internal server erro  : database updation failed");
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when Adding Employer profile  : {ex.Message}", ex);
            }

        }

       public async Task<EditEmployerProfileDto> UpdateEmployerProfile(Guid userId, EditEmployerProfileDto employerProfileDto)
        {
            try
            {
                var existingEmployer = await  _employerRepository.GetEmployerByIdAsync(userId) ?? throw new Exception("Labour not found");

                existingEmployer.FullName = employerProfileDto.FullName??existingEmployer.FullName;

                if(employerProfileDto.PreferedMuncipalityId != null)
                {
                    existingEmployer.PreferedMuncipalityId = employerProfileDto.PreferedMuncipalityId;
                }

              var result =  await _employerRepository.UpdateEmployer(existingEmployer);
                return employerProfileDto;


            }
            catch (Exception ex)
            {
                throw new Exception($"Error when Updating Employer Profile {ex.Message}", ex);
            }
        }
    }
}
