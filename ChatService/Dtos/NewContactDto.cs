using System.ComponentModel.DataAnnotations;

namespace ChatService.Dtos
{
    public class NewContactDto
    {

        [Required]
        public Guid User2Id { get; set; }

        [Required]
        public string User1Name { get; set; }

        [Required]

        public string User2Name { get; set; }


        public string? User1Image { get; set; }

        public string? User2Image { get; set; }

        public string LastMessage { get; set; }
    }
}
