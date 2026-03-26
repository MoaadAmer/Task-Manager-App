using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
