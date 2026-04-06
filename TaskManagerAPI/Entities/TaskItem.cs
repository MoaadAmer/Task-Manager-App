namespace TaskManagerAPI.Entities
{
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed
    }
    public class TaskItem
    {
        public string Title { get; set; }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }

        public Guid UserId { get; set; }


        public TaskItem(string title,string description,DateTime dueDate)
        {
            Id= Guid.NewGuid();
            Title=title;
            Description=description;
            DueDate=dueDate;
            Status = TaskStatus.Pending;
        }
    }
    
}
