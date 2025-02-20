using ProfileService.Dtos;

namespace ProfileService.Services.EmployerService
{
    public interface IEmployerService
    {
        Task<CompleteEmployerProfileDto> CompleteEmployerProfile(Guid userId ,CompleteEmployerProfileDto employerProfileDto);

        Task<EditEmployerProfileDto> UpdateEmployerProfile(Guid userId, EditEmployerProfileDto employerProfileDto);
    }
}
