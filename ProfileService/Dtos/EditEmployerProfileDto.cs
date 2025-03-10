namespace ProfileService.Dtos
{
    public class EditEmployerProfileDto
    {
        public string? FullName { get; set; }
        public string? PreferedMuncipality { get; set; }

        public IFormFile? Image { get; set; }
    }
}
