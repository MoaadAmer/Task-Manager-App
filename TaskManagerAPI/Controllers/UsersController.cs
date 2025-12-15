using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entites;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UsersController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<GetUserDTO>> Create(CreateUserDTO user)
        {
            User newUser = await _userRepo.Create(user);
            return CreatedAtAction(nameof(GetById), new { Id = newUser.Id }, new GetUserDTO()
            {
                Id = newUser.Id,
                Email = newUser.Email,
                FullName = newUser.FullName

            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> GetById(Guid id)
        {
            User user = await _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<GetUserDTO>> GetAll()
        {
            List<User> users = await _userRepo.GetAll();
            return Ok(users);
        }
    }
}
