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
            return CreatedAtAction(nameof(GetById), new { Id = newUser.Id }, UserToGetUserDTO(newUser));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> GetById(Guid id)
        {
            User? user = await _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(UserToGetUserDTO(user));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAll()
        {
            IEnumerable<GetUserDTO> users = (await _userRepo.GetAll()).Select(UserToGetUserDTO);
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDTO updateUserDTO)
        {
            if (await _userRepo.GetById(id) != null)
            {
                await _userRepo.Update(id, updateUserDTO);

                return NoContent();
            }
            return NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _userRepo.GetById(id) != null)
            {
                await _userRepo.Delete(id);

                return NoContent();
            }
            return NotFound();
        }

        private GetUserDTO UserToGetUserDTO(User newUser)
        {
            return new GetUserDTO()
            {
                Id = newUser.Id,
                Email = newUser.Email,
                FullName = newUser.FullName
            };
        }

    }
}
