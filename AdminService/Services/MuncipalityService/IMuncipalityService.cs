using AdminService.Dtos.MuncipalityDtos;
using AdminService.Helpers.Common;
using AdminService.Models;

namespace AdminService.Services.MuncipalityService
{
    public interface IMuncipalityService
	{
		Task<ApiResponse<List<MuncipalityViewDto>>> GetAll();
		Task<ApiResponse<string>> AddMuncipality(MuncipalityViewDto muncipality);
		Task<ApiResponse<MuncipalityViewDto>> GetMuncipalityById(int id);
		Task<ApiResponse<string>> DeleteMuncipality(int id);
		Task<ApiResponse<string>> UpdateMuncipality(MuncipalityViewDto muncipality);
		Task<ApiResponse<List<MuncipalityViewDto>>> GetMuncipalitiesByState(string state);
		Task<ApiResponse<List<MuncipalityViewDto>>> GetAllMuncipality();

		Task<ApiResponse<string>> ActivateMuncipality(int id);

        Task<ApiResponse<List<MuncipalityViewDto>>> GetMuncipalityBySearchParams(string searchParams);



    }
}
