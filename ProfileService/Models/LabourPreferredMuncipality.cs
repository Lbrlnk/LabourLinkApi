using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.Models
{
    public class LabourPreferredMuncipality
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "LabourId is required.")]
        public Guid LabourId { get; set; }

        [Required(ErrorMessage = "MunicipalityId is required.")]
        
        public string MunicipalityName { get; set; }

        public Labour Labour { get; set; }


    }
}
