using AdminService.Dtos;
using AdminService.Models;

namespace AdminService.Repository.MuncipalityRepository
{
	public interface IMuncipalityRepository
	{
		Task<List<Muncipality>> GetMuncipalitiesAsync();
		Task<Muncipality> AddMuncipalityAsync(Muncipality muncipality);
		Task<Muncipality> GetMuncipalityByIdAsync(int id);
		Task<bool> UpdateMuncipalityAsync(Muncipality muncipality);
		Task<List<Muncipality>> GetMuncipalityByStateAsync(string state);
	}
}
