using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class Muncipality
    {

        [Key]
        public int MunicipalityId { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsActive {  get; set; }=true;
    }
}
