using Microsoft.AspNetCore.Mvc;
using Controller_based_APIs.Models;
using Controller_based_APIs.Services;

namespace Controller_based_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {

        [HttpPost]
        public ActionResult<TodoItemDTO> AddItem(TodoItemForCreationDTO item)
        {
            var todoItem = new TodoItemDTO
            {
                Name = item.Name,
                IsComplete = item.IsComplete
            };

            TodoItemsService.Add(todoItem);
            return CreatedAtAction(nameof(GetItem), new { id = todoItem.Id }, todoItem);
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItemDTO>> GetItems()
        {
            return Ok(TodoItemsService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItemDTO> GetItem(int id)
        {
            var todoItem = TodoItemsService.Get(id);

            return todoItem == null ?
                NotFound() :
                Ok(todoItem);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, TodoItemForUpdateDTO item)
        {
            if (!TodoItemsService.Exists(id))
            {
                return NotFound();
            }

            var todoItem = new TodoItemDTO
            {
                Id = id,
                Name = item.Name,
                IsComplete = item.IsComplete
            };

            TodoItemsService.Update(todoItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            if (!TodoItemsService.Exists(id))
            {
                return NotFound();
            }
            TodoItemsService.Delete(id);

            return NoContent();
        }

    }
}
