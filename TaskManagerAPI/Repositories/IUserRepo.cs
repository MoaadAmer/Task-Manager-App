
namespace TaskManagerAPI.Repositories;

using System.Collections.Generic;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;

public interface IUserRepo
{
    Task Insert(User user);
    Task<User?> GetById(Guid id);
    Task<User?> GetByEmail(string email);

    Task<List<User>> GetAll();
    Task Update(Guid id, UpdateUserDTO updateUserDTO);
    Task Delete(Guid id);
}
