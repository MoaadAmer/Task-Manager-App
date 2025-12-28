using Microsoft.AspNetCore.Identity;
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
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(IUserRepo userRepo, IPasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            User? user = await _userRepo.GetByEmail(loginDTO.Email);
            if (user != null && _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password) == PasswordVerificationResult.Success)
            {
                return Ok();
            }
            return Unauthorized();

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
