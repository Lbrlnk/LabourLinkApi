using ProfileService.Dtos;
using ProfileService.Helpers.ApiResponse;

namespace ProfileService.Services.EmployerService
{
    public interface IEmployerService
    {
        Task<string> CompleteEmployerProfile(Guid userId ,CompleteEmployerProfileDto employerProfileDto);

        Task<string> UpdateEmployerProfile(Guid userId, EditEmployerProfileDto employerProfileDto);

        Task<EmployerView> GetEmployerDetails(Guid userId);

        Task<ApiResponse<List<EmployerView>>> GetAllEmployers();
        Task<ApiResponse<int>> CountEmployers();

	}
}
