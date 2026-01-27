
using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Entites;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class InMemoryUserRepo : IUserRepo
    {
        private List<User> _users = [];

        private readonly IPasswordHasher<User> _passwordHasher;
        public InMemoryUserRepo(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
            var adminUser = new User()
            {
                Email = "Moaad@gmail.com",
                FullName = "Moaad amer",
                Id = new Guid()
            };
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "123456");
            _users.Add(adminUser);
        }

        public Task<User> Create(CreateUserDTO user)
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                FullName = user.FullName,
                Email = user.Email
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, user.Password);
            _users.Add(newUser);
            return Task.FromResult(newUser);
        }

        public Task<List<User>> GetAll()
        {
            return Task.FromResult(_users);
        }

        public Task<User?> GetById(Guid id)
        {
            User? user = _users.Find(user => user.Id == id);
            return Task.FromResult(user);
        }

        public async Task Update(Guid id, UpdateUserDTO updateUserDTO)
        {
            User? user = await GetById(id);
            if (user != null)
            {
                user.FullName = updateUserDTO.FullName;
            }
        }

        public async Task Delete(Guid id)
        {
            int index = _users.FindIndex(user => user.Id == id);
            if (index >= 0)
            {
                _users.RemoveAt(index);
            }
            await Task.Delay(1);
        }

        public Task<User?> GetByEmail(string email)
        {
            return Task.FromResult(_users.Find(user => user.Email == email));
        }
    }
}
