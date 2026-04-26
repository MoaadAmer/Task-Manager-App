using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Models.User
{
    public class UpdateUserRequest
    {
        public string FullName { get; set; }
        public UserRole Role { get; set; }
    }
}