using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services.Interfaces;
using System.Security.Claims;

namespace CourseManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _service;

        public InstructorController(IInstructorService service)
        {
            _service = service;
        }


        // CREATE MY PROFILE
        [HttpPost("me")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CreateMyProfile(CreateInstructorDto dto)
        {
            var userId =
                User.FindFirst("UserId")?.Value;

            await _service.CreateAsync(dto, userId);

            return Ok("Instructor profile created successfully");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok("Instructor deleted");
        }
    }
}