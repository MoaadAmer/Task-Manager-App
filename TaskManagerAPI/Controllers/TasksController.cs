using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models.Task;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            {
                return Unauthorized();
            }
            TaskItem? taskItem = await _taskRepo.GetById(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            if (User.IsInRole(UserRole.Admin.ToString()))
            {
                return Ok(taskItem);
            }
            if (userId != taskItem.UserId)
            {
                return Forbid();
            }
            return Ok(taskItem);
        }
        [HttpGet()]
        public async Task<ActionResult<List<TaskItem>>> GetAll()
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            {
                return Unauthorized();
            }
            var tasks = await _taskRepo.GetAll();

            if (User.IsInRole(UserRole.Admin.ToString()))
            {
                return Ok(tasks);
            }
            return Ok(tasks.Where(x => x.UserId == userId));
        }
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(CreateTaskRequest request)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            {
                return Unauthorized();
            }
            var newTask = new TaskItem(userId, request.Title, request.Description, request.DueDate);
            await _taskRepo.Insert(newTask);

            return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            {
                return Unauthorized();
            }
            TaskItem? taskItem = await _taskRepo.GetById(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            if (!User.IsInRole(UserRole.Admin.ToString()) && userId != taskItem.UserId)
            {
                return Forbid();
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
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            {
                return Unauthorized();
            }
            TaskItem? taskItem = await _taskRepo.GetById(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            if (!User.IsInRole(UserRole.Admin.ToString()) && userId != taskItem.UserId)
            {
                return Forbid();
            }
            await _taskRepo.Delete(id);
            return NoContent();
        }
    }
}
