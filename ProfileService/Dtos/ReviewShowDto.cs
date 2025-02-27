namespace ProfileService.Dtos
{
	public class ReviewShowDto
	{
		public decimal Rating { get; set; }
		public string? Comment { get; set; }
		public string Image { get; set; }
		public string FullName { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
