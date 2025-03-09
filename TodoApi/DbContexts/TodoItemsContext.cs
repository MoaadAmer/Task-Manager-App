using Controller_based_APIs.Entities;
using Controller_based_APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace Controller_based_APIs.DbContexts
{
    public class TodoItemsContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public TodoItemsContext(DbContextOptions<TodoItemsContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = 1,
                    Name = "Item 1",
                    IsComplete = false,
                    StartDate = DateTime.Now.AddDays(1),
                    DueDate = DateTime.Now.AddDays(2)
                },
                new TodoItem
                {
                    Id = 2,
                    Name = "Item 2",
                    IsComplete = false,
                    StartDate = DateTime.Now.AddDays(2),
                    DueDate = DateTime.Now.AddDays(3)
                },
                new TodoItem
                {
                    Id = 3,
                    Name = "Item 3",
                    IsComplete = false,
                    StartDate = DateTime.Now.AddDays(3),
                    DueDate = DateTime.Now.AddDays(4)
                }
                );
            base.OnModelCreating(modelBuilder);

        }
    }


}
