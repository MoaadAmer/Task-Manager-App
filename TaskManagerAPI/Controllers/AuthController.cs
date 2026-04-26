using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models.Auth;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Base
    {
        private readonly IUserRepo _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        IPasswordService _passwordService;

        private readonly int _refreshTokenExpirationDays;

        public AuthController(IUserRepo userRepo,
            ITokenService tokenService,
            IRefreshTokenRepo refreshTokenRepo,
            IPasswordService passwordService,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _refreshTokenRepo = refreshTokenRepo;
            _passwordService = passwordService;
            _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"]);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest createUserDTO)
        {
            User? user = await _userRepo.GetByEmail(createUserDTO.Email);
            if (user != null)
            {
                return BadRequest("Email is already registered");
            }
            user = new User { Id = Guid.NewGuid(), Email = createUserDTO.Email, FullName = createUserDTO.FullName };
            user.PasswordHash = _passwordService.HashPassword(user, createUserDTO.Password);

            await _userRepo.Insert(user);

            string accessToken = _tokenService.CreateAccessToken(user);
            string refreshToken = _tokenService.CreateRefreshToken();
            await _refreshTokenRepo.SaveToken(
                new RefreshToken(refreshToken, user.Id, _refreshTokenExpirationDays)
                );

            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            User? user = await _userRepo.GetByEmail(loginRequest.Email);
            if (user != null && _passwordService.VerifyPassword(user, loginRequest.Password))
            {
                string accessToken = _tokenService.CreateAccessToken(user);
                string refreshToken = _tokenService.CreateRefreshToken();

                await
                    _refreshTokenRepo.
                    SaveToken(
                    new RefreshToken(refreshToken, user.Id, _refreshTokenExpirationDays)
                    );

                return Ok(new { accessToken, refreshToken });
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(RefreshTokenRequest refreshTokenRequest)
        {
            Guid? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            string refreshToken = refreshTokenRequest.Token;
            RefreshToken? storedToken = await _refreshTokenRepo.Get(refreshToken);

            if (storedToken == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            if (storedToken.UserId! == userId)
            {
                return Forbid();
            }
            if (storedToken.Revoked)
            {
                return Unauthorized("Refresh token revoked");
            }
            storedToken.Revoke();
            await _refreshTokenRepo.Update(storedToken);

            return Ok(new { message = "Logged out successfully." });

        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            RefreshToken? storedToken = await _refreshTokenRepo.Get(refreshTokenRequest.Token);
            if (storedToken == null)
                return Unauthorized("Invalid refresh token");

            if (storedToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            if (storedToken.Revoked)
                return Unauthorized("Refresh token already revoked");

            var user = await _userRepo.GetById(storedToken.UserId);
            if (user == null)
                return Unauthorized("User no longer exists");

            string newRefreshToken = _tokenService.CreateRefreshToken();

            await _refreshTokenRepo.SaveToken(
                new RefreshToken(newRefreshToken, user.Id, _refreshTokenExpirationDays)
                );
            storedToken.Revoke(newRefreshToken);
            await _refreshTokenRepo.Update(storedToken);

            string newAccessToken = _tokenService.CreateAccessToken(user);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }
    }
}
