using System.Security.Claims;
using JobPortalAPI.Data;
using JobPortalAPI.DTOs;
using JobPortalAPI.Models;
using JobPortalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtService _jwt;
        
        public AuthController(ApplicationDbContext db, IPasswordHasher<User> hasher, JwtService jwt)
        {
            _db = db;
            _hasher = hasher;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("This email is on the list");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = dto.Role ?? "User"
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized("User not found");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash!, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Incorrect password");

            var token = _jwt.GenerateToken(user);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok($"Yours ID: {userId}");
        }

        [Authorize]
        [HttpGet("admin-area")]
        public IActionResult AdminOnly()
        {
            return Ok("Only admins can see it.");
        }
    }
}
