using TaskManagerAPI.Entities;
using TaskManagerAPI.Repositories.Interfaces;

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
            return Task.FromResult(_users.FirstOrDefault(user => user.Id == id));

        }

        public Task Update(User user)
        {
            return Task.CompletedTask;
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
            return Task.FromResult(_users.FirstOrDefault(user => user.Email == email));
        }
    }
}
