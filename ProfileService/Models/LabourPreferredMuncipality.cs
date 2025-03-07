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
        [Range(1, int.MaxValue, ErrorMessage = "MunicipalityId must be greater than 0.")]
        public int MunicipalityId { get; set; }

        public Labour Labour { get; set; }


    }
}
