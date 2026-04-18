using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using CourseManagementAPI.DTOs.Auth;

namespace CourseManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(
            UserManager<User> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }


        // ================= REGISTER =================

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (dto.Role != "Admin"
                && dto.Role != "Student"
                && dto.Role != "Instructor")
            {
                return BadRequest("Invalid role");
            }

            var existingUser =
                await _userManager.FindByNameAsync(dto.Username);

            if (existingUser != null)
                return BadRequest("Username already exists");

            var user = new User
            {
                UserName = dto.Username,
                Role = dto.Role
            };

            var result =
                await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }


        // ================= LOGIN =================

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user =
                await _userManager.FindByNameAsync(dto.Username);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var validPassword =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password
                );

            if (!validPassword)
                return Unauthorized("Invalid credentials");

            var token =
                _jwtService.GenerateToken(user);

            Response.Cookies.Append(
                "jwt",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires =
                        DateTimeOffset.UtcNow.AddHours(2)
                });

            return Ok("Login successful");
        }


        // ================= LOGOUT =================

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok("Logged out successfully");
        }
    }
}