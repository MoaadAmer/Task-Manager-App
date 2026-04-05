
namespace TaskManagerAPI.Repositories;

using System.Collections.Generic;
using TaskManagerAPI.Entities;

public interface IUserRepo
{
    Task Insert(User user);
    Task<User?> GetById(Guid id);
    Task<User?> GetByEmail(string email);
    Task<List<User>> GetAll();
    Task Update(User user);
    Task Delete(Guid id);
}
