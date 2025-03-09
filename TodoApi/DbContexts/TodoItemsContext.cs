using Controller_based_APIs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Controller_based_APIs.DbContexts
{
    public class TodoItemsContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public TodoItemsContext(DbContextOptions options) : base(options)
        {

        }
    }
}
