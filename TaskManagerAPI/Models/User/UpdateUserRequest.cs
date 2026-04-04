using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models.User
{
    public class UpdateUserRequest
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
    }
}