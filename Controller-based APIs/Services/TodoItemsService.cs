using Controller_based_APIs.Models;

namespace Controller_based_APIs.Services
{
    public static class TodoItemsService
    {
        private static List<TodoItemDTO> TodoItems { get; set; } = [
            new TodoItemDTO { Id = 1, Name = "Item 1", IsComplete = false },
            new TodoItemDTO { Id = 2, Name = "Item 2", IsComplete = false },
            new TodoItemDTO { Id = 3, Name = "Item 3", IsComplete = false }
        ];

        public static void Add(TodoItemDTO item)
        {
            int id = TodoItems.Count + 1;
            item.Id = id;
            TodoItems.Add(item);
        }

        public static List<TodoItemDTO> GetAll()
        {
            return TodoItems;
        }

        public static TodoItemDTO? Get(int id)
        {
            return TodoItems.FirstOrDefault(item => item.Id == id);
        }

        public static void Update(TodoItemDTO item)
        {
            int index = TodoItems.FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                TodoItems[index] = item;
            }
        }

        public static void Delete(TodoItemDTO item)
        {
            int index = TodoItems.FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                TodoItems.RemoveAt(index);
            }
        }
    }
}
