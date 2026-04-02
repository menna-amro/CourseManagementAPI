using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services.Interfaces;

namespace CourseManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Student,Admin")]
        public async Task<IActionResult> Enroll(CreateEnrollmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _enrollmentService.EnrollAsync(dto);
            return Ok("Student enrolled successfully");
        }

        [HttpGet("student/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentCourses(int studentId)
        {
            var result = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(result);
        }

        [HttpGet("course/{courseId}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> GetCourseStudents(int courseId)
        {
            var result = await _enrollmentService.GetCourseStudentsAsync(courseId);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int studentId, int courseId)
        {
            await _enrollmentService.DeleteAsync(studentId, courseId);
            return Ok("Enrollment removed successfully");
        }
    }
}