using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models.Task;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepo _taskRepo;

        public TasksController(ITaskRepo taskRepo)
        {
            _taskRepo = taskRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem?>> GetById(Guid id)
        {
            TaskItem? item = await _taskRepo.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpGet("")]
        public async Task<ActionResult<List<TaskItem>>> GetAll()
        {
            return await _taskRepo.GetAll();
        }
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(CreateTaskRequest request)
        {
            var newTask = new TaskItem(request.Title, request.Description, request.DueDate);
            await _taskRepo.Insert(newTask);

            return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request)
        {
            TaskItem? taskItem = await _taskRepo.GetById(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                taskItem.Title = request.Title;
            }
            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                taskItem.Description = request.Description;
            }

            taskItem.DueDate = request.DueDate;
            taskItem.Status = request.Status;

            await _taskRepo.Update(taskItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            TaskItem? taskItem = await _taskRepo.GetById(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            await _taskRepo.Delete(id);
            return NoContent();
        }
    }
}
