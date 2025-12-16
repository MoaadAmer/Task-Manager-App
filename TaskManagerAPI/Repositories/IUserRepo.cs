namespace TaskManagerAPI.Repositories;

using System.Collections.Generic;
using TaskManagerAPI.Entites;
using TaskManagerAPI.Models;

public interface IUserRepo
{
    Task<User> Create(CreateUserDTO user);
    Task<User?> GetById(Guid id);

    Task<List<User>> GetAll();
    Task Update(Guid id, UpdateUserDTO updateUserDTO);
}
