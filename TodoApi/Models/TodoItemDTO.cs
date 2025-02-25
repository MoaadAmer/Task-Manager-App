namespace Controller_based_APIs.Models
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}
