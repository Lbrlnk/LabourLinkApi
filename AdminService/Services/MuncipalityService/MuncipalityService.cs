using AdminService.Data;
using AdminService.Dtos;
using AdminService.Helpers.Common;
using AdminService.Models;
using AdminService.Repository.MuncipalityRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Services.MuncipalityService
{
	public class MuncipalityService : IMuncipalityService
	{
		private readonly IMuncipalityRepository _repository;
		private readonly AdminDbContext _context;
		private readonly IMapper _mapper;
		public MuncipalityService(IMuncipalityRepository repository, AdminDbContext context,IMapper mapper)
		{
			_repository = repository;
			_context = context;
			_mapper = mapper;
		}
		public async Task<ApiResponse<List<MuncipalityViewDto>>> GetAll()
		{
			var result = await _repository.GetMuncipalitiesAsync();
			var res = _mapper.Map<List<MuncipalityViewDto>>(result);
			return new ApiResponse<List<MuncipalityViewDto>>(200,"success", res);
		}
		public async Task<ApiResponse<string>> AddMuncipality(MuncipalityViewDto muncipality)
		{
			var mun = _mapper.Map<Muncipality>(muncipality);
			var result = await _repository.AddMuncipalityAsync(mun);
			if (result != null)
			{
				return new ApiResponse<string>(200, "success", data: $"Muncipality added succesfully {result.MunicipalityId}");
			}

			return new ApiResponse<string>(500, "internal server error  ", error: "error occured during the updation of the database");

		}
		public async Task<ApiResponse<MuncipalityViewDto>> GetMuncipalityById(int id)
		{
			var res = await _repository.GetMuncipalityByIdAsync(id);
			var mun=_mapper.Map<MuncipalityViewDto>(res);
			return new ApiResponse<MuncipalityViewDto>(200, "success", mun);
		}
		public async Task<ApiResponse<string>> DeleteMuncipality(int id)
		{ 
			var muncipality = await _repository.GetMuncipalityByIdAsync(id);
			if (muncipality != null)
			{
				muncipality.IsActive=false;
				var res=_mapper.Map<Muncipality>(muncipality);
				var Updated = await _repository.UpdateMuncipalityAsync(muncipality);
				return new ApiResponse<string>(200, "success", "Muncipality removed ");
			}
			return new ApiResponse<string>(404, "not found", error: "Muncipality canot found ");
		}
		public async Task<ApiResponse<string>> UpdateMuncipality(MuncipalityViewDto muncipality)
		{
			int id = muncipality.MunicipalityId;
			var exmuncipality = await _repository.GetMuncipalityByIdAsync(id);
			if (exmuncipality != null)
			{
				exmuncipality.Name = muncipality.Name ?? exmuncipality.Name;
				exmuncipality.State = muncipality.State ?? exmuncipality.State;
				var updated = await _repository.UpdateMuncipalityAsync(exmuncipality);
				if (updated)
				{
					return new ApiResponse<string>(200, "success", $"Muncipality updated successfully ");
				}
			}
			return new ApiResponse<string>(404, "Not Found", error: $"Invalid Input Muncipality");
		}
		public async Task<ApiResponse<List<MuncipalityViewDto>>> GetMuncipalitiesByState(string state)
		{
			var response = await _repository.GetMuncipalityByStateAsync(state);
			var res = _mapper.Map<List<MuncipalityViewDto>>(response);
			if (response == null)
			{
				return new ApiResponse<List<MuncipalityViewDto>>(204, "Not Found");
			}
			return new ApiResponse<List<MuncipalityViewDto>>(200,"Success",res);
		}

	}
}
