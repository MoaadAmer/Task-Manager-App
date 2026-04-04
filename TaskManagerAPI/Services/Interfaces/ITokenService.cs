using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
        string CreateRefreshToken();
    }
}
