using TaskStatus = TaskManagerAPI.Entities.TaskStatus;

namespace TaskManagerAPI.Models.Task
{
    public class UpdateTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
    }
}
