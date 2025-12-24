using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class UpdateUserDTO
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
    }
}