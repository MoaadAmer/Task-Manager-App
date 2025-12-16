
using TaskManagerAPI.Models;
using TaskManagerAPI.Entites;

namespace TaskManagerAPI.Repositories
{
    public class InMemoryUserRepo : IUserRepo
    {
        private List<User> _users = [];
        public Task<User> Create(CreateUserDTO user)
        {
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                FullName = user.FullName,
                Email = user.Email,
                PasswordHash = user.Password
            };
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
    }
}
