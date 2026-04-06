using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface IRefreshTokenRepo
    {
        Task SaveToken(RefreshToken token);
        Task<RefreshToken?> Get(string token);
        Task Update(RefreshToken token);
    }
}
