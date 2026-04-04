using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public PasswordService(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }
        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }
        public bool VerifyPassword(User user, string password)
        {
            return
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password)
                == PasswordVerificationResult.Success;
        }
    }
}
