using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models.User;
using TaskManagerAPI.Repositories.Interfaces;
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
        public async Task<ActionResult<GetUserResponse>> Create(CreateUserRequest createUserRequest)
        {
            User? existingUser = await _userRepo.GetByEmail(createUserRequest.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = createUserRequest.Email,
                FullName = createUserRequest.FullName,
                Role = createUserRequest.Role,
            };
            user.PasswordHash = _passwordService.HashPassword(user, createUserRequest.Password);

            await _userRepo.Insert(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, UserToGetUserDTO(user));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserResponse>> GetById(Guid id)
        {
            User? user = await _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(UserToGetUserDTO(user));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserResponse>>> GetAll()
        {
            IEnumerable<GetUserResponse> users = (await _userRepo.GetAll()).Select(UserToGetUserDTO);
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserRequest updateUserRequest)
        {
            User? user = await _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(updateUserRequest.FullName))
            {
                user.FullName = updateUserRequest.FullName;
            }
            if (!string.IsNullOrWhiteSpace(updateUserRequest.Role))
            {
                user.Role = updateUserRequest.Role;
            }
            await _userRepo.Update(user);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _userRepo.GetById(id) == null)
            {
                return NotFound();
            }
            await _userRepo.Delete(id);
            return NoContent();
        }

        private GetUserResponse UserToGetUserDTO(User newUser)
        {
            return new GetUserResponse()
            {
                Id = newUser.Id,
                Email = newUser.Email,
                FullName = newUser.FullName,
                Role = newUser.Role,
            };
        }

    }
}
