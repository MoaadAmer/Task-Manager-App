using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models.User;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordService _passwordService;

        public UsersController(IUserRepo userRepo, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
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
            Guid? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            if (!User.IsInRole(nameof(UserRole.Admin)) && userId != id)
            {
                return Forbid();
            }

            User? user = await _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(UserToGetUserDTO(user));
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<IEnumerable<GetUserResponse>>> GetAll()
        {
            IEnumerable<GetUserResponse> users = (await _userRepo.GetAll()).Select(UserToGetUserDTO);
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserRequest updateUserRequest)
        {
            Guid? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            bool isAdmin = User.IsInRole(nameof(UserRole.Admin));
            if (!isAdmin && userId != id)
            {
                return Forbid();
            }
            User? user = await _userRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(updateUserRequest.FullName))
            {
                user.FullName = updateUserRequest.FullName;
            }
            if (Enum.IsDefined(typeof(UserRole), updateUserRequest.Role))
            {
                //admins cant update his or other admins role only other users Role
                if ((isAdmin && userId != id && user.Role != UserRole.Admin))
                {
                    user.Role = updateUserRequest.Role;
                }
            }
            await _userRepo.Update(user);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Guid? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            if (!User.IsInRole(nameof(UserRole.Admin)) && userId != id)
            {
                return Forbid();
            }
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

        private Guid? GetUserId()
        {
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            {
                return userId;
            }
            return null;
        }
    }
}
