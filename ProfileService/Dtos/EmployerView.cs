using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class EmployerView
    {
        
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string PreferedMunicipality { get; set; }
		public string? ProfileImageUrl { get; set; }
	}
}
