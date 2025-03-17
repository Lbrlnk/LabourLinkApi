using ProfileService.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models
{
    public class Labour
    {
        [Key]
        public Guid LabourId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        public LabourPreferedTime PreferedTime { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal Rating { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string? ProfilePhotoUrl { get; set; }

        [MaxLength(500, ErrorMessage = "About Yourself must be at most 500 characters.")]
        public string? AboutYourSelf { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;

        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<LabourWorkImage?> LabourWorkImages { get; set; }
        public ICollection<LabourPreferredMuncipality> LabourPreferredMunicipalities { get; set; }


        public ICollection<LabourSkills> LabourSkills { get; set; }
		public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Conversation> ConversationsAsUser2 { get; set; }
    }
}
