using ProfileService.Dtos;
using ProfileService.Helpers.ApiResponse;

namespace ProfileService.Repositories.LabourWithinEmployer
{
	public interface IEmployerLabour
	{
		Task<ApiResponse<List<LabourViewDto>>> GetLabourByEmployerMun(Guid userid, int pageNumber, int pageSize);
	}
}
