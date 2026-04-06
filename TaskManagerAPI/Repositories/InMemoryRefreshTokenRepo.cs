using TaskManagerAPI.Entities;
using TaskManagerAPI.Repositories.Interfaces;

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

        public Task Update(RefreshToken token)
        {
            return Task.CompletedTask;
        }
    }
}
