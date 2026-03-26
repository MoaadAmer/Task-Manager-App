using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string password);
    }
}
