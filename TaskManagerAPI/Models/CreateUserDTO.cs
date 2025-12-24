using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}