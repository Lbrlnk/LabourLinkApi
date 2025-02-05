using System.ComponentModel.DataAnnotations;

namespace AdminService.Dtos
{
	public class MuncipalityViewDto
	{
		[Key]
		public int MunicipalityId { get; set; }
		public string? Name { get; set; }
		public string? State { get; set; }
		public bool IsActive { get; internal set; }
	}
}
