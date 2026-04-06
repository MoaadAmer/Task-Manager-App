using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface ITaskRepo
    {
        Task Insert(TaskItem task);
        Task<TaskItem?> GetById(Guid id);
        Task<List<TaskItem>> GetAll();
        Task Update(TaskItem task);
        Task Delete(Guid id);
    }
}
