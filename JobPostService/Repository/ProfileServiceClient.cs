using JobPostService.Helpers.ApiResonse;
using System.Text.Json;

namespace JobPostService.Repository
{
	public class ProfileServiceClient
	{
		private readonly HttpClient _httpClient;

		public ProfileServiceClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ApiResponse<EmployerView>> GetClientByIdAsync(Guid clientId)
		{
			try
			{
				Console.WriteLine(clientId);
				string url = $"https://localhost:7202/api/Employer/getemployerbyid?userId={clientId}";
				var response = await _httpClient.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Raw JSON Response: {jsonResponse}");

					// Deserialize directly to EmployerView
					var client = JsonSerializer.Deserialize<EmployerView>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

					Console.WriteLine(client.FullName);

					return new ApiResponse<EmployerView>(200, "Success", client);
				}

				return new ApiResponse<EmployerView>((int)response.StatusCode, $"Error from Profile Service: {response.ReasonPhrase}");
			}
			catch (Exception ex)
			{
				return new ApiResponse<EmployerView>(500, "An error occurred while fetching client details.");
			}
		}

	}
	public class EmployerView
	{

		public string FullName { get; set; }

		public string PhoneNumber { get; set; }

		public string PreferedMunicipality { get; set; }
		public string? ProfileImageUrl { get; set; }
	}
}
