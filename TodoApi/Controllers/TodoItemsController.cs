using Microsoft.AspNetCore.Mvc;
using Controller_based_APIs.Models;
using Microsoft.AspNetCore.JsonPatch;
using Controller_based_APIs.Data;

namespace Controller_based_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IRepository<TodoItemDTO> todoRepo;

        public TodoItemsController(IRepository<TodoItemDTO> todoRepo)
        {
            this.todoRepo = todoRepo;
        }

        [HttpPost]
        public ActionResult<TodoItemDTO> AddItem(TodoItemForCreationDTO item)
        {
            var todoItem = new TodoItemDTO
            {
                Name = item.Name,
                IsComplete = item.IsComplete
            };

            todoRepo.Add(todoItem);
            return CreatedAtAction(nameof(GetItem), new { id = todoItem.Id }, todoItem);
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItemDTO>> GetItems()
        {
            return Ok(todoRepo.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<TodoItemDTO> GetItem(int id)
        {
            var todoItem = todoRepo.Get(id);

            return todoItem == null ?
                NotFound() :
                Ok(todoItem);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, TodoItemForUpdateDTO item)
        {
            if (!todoRepo.Exists(id))
            {
                return NotFound();
            }

            var todoItem = new TodoItemDTO
            {
                Id = id,
                Name = item.Name,
                IsComplete = item.IsComplete
            };

            todoRepo.Update(todoItem);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateItem(int id, JsonPatchDocument<TodoItemForUpdateDTO> patchDoc)
        {
            if (patchDoc != null)
            {
                if (!todoRepo.Exists(id))
                {
                    return NotFound();
                }
                else
                {

                    TodoItemForUpdateDTO todoItemToPatch = new();

                    patchDoc.ApplyTo(todoItemToPatch, ModelState);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!TryValidateModel(todoItemToPatch))
                    {
                        return BadRequest(ModelState);
                    }
                    var todoItem = new TodoItemDTO
                    {
                        Id = id,
                        Name = todoItemToPatch.Name,
                        IsComplete = todoItemToPatch.IsComplete
                    };

                    todoRepo.Update(todoItem);

                    return NoContent();

                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            if (!todoRepo.Exists(id))
            {
                return NotFound();
            }
            todoRepo.Delete(id);

            return NoContent();
        }

    }
}
