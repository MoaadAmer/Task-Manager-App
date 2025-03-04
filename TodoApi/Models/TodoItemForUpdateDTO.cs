using System.ComponentModel.DataAnnotations;

namespace Controller_based_APIs.Models
{
    public class TodoItemForUpdateDTO
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime DueDate { get; set; }
    }
}