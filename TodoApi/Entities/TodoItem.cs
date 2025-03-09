using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Controller_based_APIs.Entities
{
    public class TodoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
    }
}
