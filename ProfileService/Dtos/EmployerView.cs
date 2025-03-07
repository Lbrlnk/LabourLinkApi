using System.ComponentModel.DataAnnotations;

namespace ProfileService.Dtos
{
    public class EmployerView
    {
        
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public int PreferedMuncipalityId { get; set; }
    }
}
