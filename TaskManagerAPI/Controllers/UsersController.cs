using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordService _passwordService;

        public UsersController(IUserRepo userRepo, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            this._passwordService = passwordService;
        }

        [HttpPost]
        public async Task<ActionResult<GetUserDTO>> Create(CreateUserDTO createUserDTO)
        {
            User user = new User { Id = Guid.NewGuid(), Email = createUserDTO.Email, FullName = createUserDTO.FullName };
            user.PasswordHash = _passwordService.HashPassword(user, createUserDTO.Password);

            await _userRepo.Insert(user);
            return CreatedAtAction(nameof(GetById), new { user.Id }, UserToGetUserDTO(user));
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
