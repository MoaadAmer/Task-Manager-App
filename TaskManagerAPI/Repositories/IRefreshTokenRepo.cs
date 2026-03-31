using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories
{
    public interface IRefreshTokenRepo
    {
        Task SaveToken(RefreshToken token);
        Task<RefreshToken?> Get(string token);
        Task Revoke(string token, string? replacedByToken = null);
    }
}
