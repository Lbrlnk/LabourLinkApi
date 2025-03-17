namespace AuthenticationService.Models
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public User User { get; set; }
    }
}
