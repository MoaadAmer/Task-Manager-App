using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class InMemoryUserRepo : IUserRepo
    {
        private List<User> _users = [];
        public Task Insert(User user)
        {
            _users.Add(user);

            return Task.CompletedTask;
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

        public Task Delete(Guid id)
        {
            int index = _users.FindIndex(user => user.Id == id);
            if (index >= 0)
            {
                _users.RemoveAt(index);
            }

            return Task.CompletedTask;
        }

        public Task<User?> GetByEmail(string email)
        {
            return Task.FromResult(_users.Find(user => user.Email == email));
        }
    }
}
