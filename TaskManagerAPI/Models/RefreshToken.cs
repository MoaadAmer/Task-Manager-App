namespace TaskManagerAPI.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Revoked { get; set; }
        public string? ReplacedByToken { get; set; }
    }
}
