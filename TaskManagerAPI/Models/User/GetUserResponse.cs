namespace TaskManagerAPI.Models.User
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
