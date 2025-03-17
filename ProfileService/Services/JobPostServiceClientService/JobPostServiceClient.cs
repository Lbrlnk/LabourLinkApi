using ProfileService.Helpers.ApiResponse;
using ProfileService.Models;
using ProfileService.Repositories.LabourRepository;
using System.Text.Json;

namespace ProfileService.Services.JobPostServiceClientService
{
	public class JobPostServiceClient
	{
		private readonly HttpClient _httpClient;
		private readonly ILabourRepository _repository;

		public JobPostServiceClient(HttpClient httpClient,ILabourRepository repository)
		{
			_httpClient = httpClient;
			_repository = repository;
		}
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> GetPrefferedJobposts(Guid userid)
		{
			try
			{
				var labour = await _repository.GetLabourByuserIdAsync(userid);

				if (labour == null || labour.LabourPreferedMuncipalities == null || labour.LabourSkills == null)
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(400, "Labourer not found or missing skills/municipalities.");
				}

				string municipalityQuery = string.Join("&municipality=", labour.LabourPreferedMuncipalities.Select(m => m.MunicipalityName));
				string skillQuery = string.Join("&skills=", labour.LabourSkills.Select(s => s.SkillName));
				string url = $"https://localhost:7299/api/Job/getjobpostbymunicipalityandskill?municipality={municipalityQuery}&skills={skillQuery}";

				var response = await _httpClient.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					var jobPosts = JsonSerializer.Deserialize<ApiResponse<List<LabourViewJobPostDto>>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					return jobPosts ?? new ApiResponse<List<LabourViewJobPostDto>>(500, "Invalid response from Job Post Service.");
				}

				return new ApiResponse<List<LabourViewJobPostDto>>((int)response.StatusCode, $"Error from Job Post Service: {response.ReasonPhrase}");
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<LabourViewJobPostDto>>(500, "An error occurred while fetching job posts.");
			}
		}
	}
	public class LabourViewJobPostDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Wage { get; set; }
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public string PrefferedTime { get; set; }
		public string MuncipalityId { get; set; }
		public string Status { get; set; }
		public string SkillId1 { get; set; }
		public string SkillId2 { get; set; }
		public string Image { get; set; }
		public DateOnly CreatedDate { get; set; }
	}
}
