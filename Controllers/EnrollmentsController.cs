using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManagementAPI.DTOs;

namespace CourseManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnrollmentsController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // POST: Enroll Student
        // =========================
        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody] CreateEnrollmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check student exists
            var studentExists = await _context.Students
                .AnyAsync(s => s.Id == dto.StudentId);

            if (!studentExists)
                return NotFound("Student not found");

            // Check course exists
            var courseExists = await _context.Courses
                .AnyAsync(c => c.Id == dto.CourseId);

            if (!courseExists)
                return NotFound("Course not found");

            // Prevent duplicate enrollment
            var exists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);

            if (exists)
                return BadRequest("Student already enrolled in this course");

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok("Student enrolled successfully");
        }

        // =========================
        // GET: Courses of a Student
        // =========================
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentCourses(int studentId)
        {
            var courses = await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Select(e => new
                {
                    CourseId = e.Course.Id,
                    CourseTitle = e.Course.Title,
                    InstructorName = e.Course.Instructor.Name
                })
                .ToListAsync();

            return Ok(courses);
        }

        // =========================
        // GET: Students in a Course
        // =========================
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseStudents(int courseId)
        {
            var students = await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.CourseId == courseId)
                .Select(e => new
                {
                    StudentId = e.Student.Id,
                    StudentName = e.Student.Name,
                    StudentEmail = e.Student.Email
                })
                .ToListAsync();

            return Ok(students);
        }

        // =========================
        // DELETE: Remove Enrollment
        // =========================
        [HttpDelete]
        public async Task<IActionResult> RemoveEnrollment(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null)
                return NotFound("Enrollment not found");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}