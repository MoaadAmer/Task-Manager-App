using Controller_based_APIs.Models;

namespace Controller_based_APIs.Data
{
    public class InMemoryTodoRepo : IRepository<TodoItemDTO>
    {
        private List<TodoItemDTO> TodoItems { get; set; } = [
            new TodoItemDTO { Id = 1, Name = "Item 1", IsComplete = false,StartDate=DateTime.Now.AddDays(1),DueDate=DateTime.Now.AddDays(2) },
            new TodoItemDTO { Id = 2, Name = "Item 2", IsComplete = false,StartDate=DateTime.Now.AddDays(2),DueDate=DateTime.Now.AddDays(3) },
            new TodoItemDTO { Id = 3, Name = "Item 3", IsComplete = false ,StartDate=DateTime.Now.AddDays(3),DueDate=DateTime.Now.AddDays(4)}
        ];

        public void Add(TodoItemDTO item)
        {
            int id = TodoItems.Count + 1;
            item.Id = id;
            TodoItems.Add(item);
        }

        public IEnumerable<TodoItemDTO> GetAll()
        {
            return TodoItems;
        }

        public TodoItemDTO? Get(int id)
        {
            return TodoItems.FirstOrDefault(item => item.Id == id);
        }

        public void Update(TodoItemDTO item)
        {
            int index = TodoItems.FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                TodoItems[index] = item;
            }
        }

        public void Delete(int id)
        {
            int index = TodoItems.FindIndex(i => i.Id == id);
            if (index != -1)
            {
                TodoItems.RemoveAt(index);
            }
        }

        public bool Exists(int id)
        {
            return TodoItems.Any(item => item.Id == id);
        }
    }
}
