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
            return CreatedAtAction(nameof(GetItem), new { id = todoItem.Id }, item);
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
        public IActionResult UpdateItem(int id, TodoItemDTO item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            var todoItem = TodoItemsService.Get(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            TodoItemsService.Update(item);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            var item = TodoItemsService.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            TodoItemsService.Delete(item);

            return NoContent();
        }

    }
}
