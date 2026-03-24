using TaskManagerAPI.Entites;

namespace TaskManagerAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
