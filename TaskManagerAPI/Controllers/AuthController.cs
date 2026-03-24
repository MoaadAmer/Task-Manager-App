using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entites;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        public AuthController(IUserRepo userRepo,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(LoginDTO loginDTO)
        //{
        //    User? user = await _userRepo.GetByEmail(loginDTO.Email);
        //    if (user != null)
        //    {
        //        return Ok(new { Token = token });
        //    }
        //    return Unauthorized();

        //} 

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            User? user = await _userRepo.GetByEmail(loginDTO.Email);
            if (user != null && _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password) == PasswordVerificationResult.Success)
            {
                string token = _tokenService.CreateToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }
    }
}
