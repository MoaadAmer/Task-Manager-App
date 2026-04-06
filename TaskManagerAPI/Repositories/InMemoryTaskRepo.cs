using TaskManagerAPI.Entities;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories
{
    public class InMemoryTaskRepo : ITaskRepo
    {
        private List<TaskItem> _tasks = [];

        public async Task Delete(Guid id)
        {
            TaskItem? item = await GetById(id);
            if (item != null)
            {
                _tasks.Remove(item);
            }
        }

        public Task<List<TaskItem>> GetAll()
        {
            return Task.FromResult(_tasks.ToList());
        }

        public Task<TaskItem?> GetById(Guid id)
        {
            return Task.FromResult(_tasks.FirstOrDefault(item => item.Id == id));
        }

        public Task Insert(TaskItem task)
        {
            _tasks.Add(task);
            return Task.CompletedTask;
        }

        public Task Update(TaskItem task)
        {
            return Task.CompletedTask;
        }
    }
}
