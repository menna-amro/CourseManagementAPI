using Microsoft.AspNetCore.Mvc;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services.Interfaces;

namespace CourseManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // POST: api/enrollments
        [HttpPost]
        public async Task<IActionResult> Enroll(CreateEnrollmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _enrollmentService.EnrollAsync(dto);
                return Ok("Student enrolled successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/enrollments/student/5
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentCourses(int studentId)
        {
            var result = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(result);
        }

        // GET: api/enrollments/course/3
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseStudents(int courseId)
        {
            var result = await _enrollmentService.GetCourseStudentsAsync(courseId);
            return Ok(result);
        }

        // DELETE: api/enrollments?studentId=1&courseId=2
        [HttpDelete]
        public async Task<IActionResult> Delete(int studentId, int courseId)
        {
            await _enrollmentService.DeleteAsync(studentId, courseId);
            return Ok("Enrollment removed successfully");
        }
    }
}