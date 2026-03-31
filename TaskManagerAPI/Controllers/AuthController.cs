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

        [HttpPost("register")]
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
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            string refreshToken = refreshTokenRequest.RefreshToken;
            // 1. Get stored refresh token
            RefreshToken? storedToken = await _refreshTokenRepo.Get(refreshToken);
            if (storedToken == null)
                return Unauthorized("Invalid refresh token");

            // 2. Check expiration
            if (storedToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            // 3. Check if revoked
            if (storedToken.Revoked)
                return Unauthorized("Refresh token revoked");

            // 4. Get associated user
            var user = await _userRepo.GetById(storedToken.UserId);
            if (user == null)
                return Unauthorized("User no longer exists");

            // 5. Rotate refresh token (Security requirement)
            string newRefreshToken = _tokenService.CreateRefreshToken();

            // Save new refresh token
            await _refreshTokenRepo.SaveToken(new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                Revoked = false
            });

            // Revoke old
            await _refreshTokenRepo.Revoke(refreshToken, newRefreshToken);

            // 6. Generate new Access token
            string newAccessToken = _tokenService.CreateAccessToken(user);

            // 7. Return new tokens to client
            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
    }
}
