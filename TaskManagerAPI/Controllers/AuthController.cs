using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        IPasswordService _passwordService;
        public AuthController(IUserRepo userRepo,
            ITokenService tokenService,
            IRefreshTokenRepo refreshTokenRepo,
            IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _refreshTokenRepo = refreshTokenRepo;
            _passwordService = passwordService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserDTO createUserDTO)
        {
            User? user = await _userRepo.GetByEmail(createUserDTO.Email);
            if (user != null)
            {
                return BadRequest("Email is already registered");
            }
            user = new User { Id = Guid.NewGuid(), Email = createUserDTO.Email, FullName = createUserDTO.FullName };
            user.PasswordHash = _passwordService.HashPassword(user, createUserDTO.Password);

            await _userRepo.Insert(user);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            User? user = await _userRepo.GetByEmail(loginDTO.Email);
            if (user != null && _passwordService.VerifyPassword(user, loginDTO.Password))
            {
                string accessToken = _tokenService.CreateAccessToken(user);
                string refreshToken = _tokenService.CreateRefreshToken();



                await _refreshTokenRepo.SaveToken(
                    new RefreshToken()
                    {
                        Token = refreshToken,
                        UserId = user.Id,
                        ExpiresAt = DateTime.UtcNow.AddDays(7),
                        Revoked = false,
                        ReplacedByToken = null
                    });

                return Ok(new { accessToken, refreshToken });
            }
            return Unauthorized();
        }
    }
}
