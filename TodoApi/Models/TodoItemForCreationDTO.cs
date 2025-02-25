using System.ComponentModel.DataAnnotations;

namespace Controller_based_APIs.Models
{
    public class TodoItemForCreationDTO
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}