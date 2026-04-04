namespace TaskManagerAPI.Entities
{
    public class RefreshToken
    {
        public string Token { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool Revoked { get; set; }
        public string? ReplacedByToken { get; set; }

        public RefreshToken(string refreshToken, Guid id, int refreshTokenExpirationDays)
        {
            Token = refreshToken;
            UserId = id;
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
            Revoked = false;
            ReplacedByToken = null;
        }

        public void Revoke(string? token = null)
        {
            Revoked = true;
            ReplacedByToken = token;
        }
    }
}
