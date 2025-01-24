using Microsoft.EntityFrameworkCore;

namespace Controller_based_APIs.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }
        public DbSet<TodoItem>? TodoItems { get; set; } = null;
    }
}
