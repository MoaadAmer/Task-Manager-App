using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class InMemoryRefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly List<RefreshToken> _tokens = new();

        public Task SaveToken(RefreshToken token)
        {
            _tokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<RefreshToken?> Get(string token)
        {
            return Task.FromResult(_tokens.FirstOrDefault(t => t.Token == token));
        }

        public Task Revoke(string token, string? replacedByToken = null)
        {
            var existing = _tokens.FirstOrDefault(t => t.Token == token);
            if (existing != null)
            {
                existing.Revoked = true;
                existing.ReplacedByToken = replacedByToken;
            }

            return Task.CompletedTask;
        }
    }
}
