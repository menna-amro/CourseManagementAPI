using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManagementAPI.DTOs;

namespace CourseManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: All Courses
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor.Name
                })
                .ToListAsync();

            return Ok(courses);
        }

        // =========================
        // GET: Course by Id
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor.Name
                })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // =========================
        // POST: Create Course
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check instructor exists
            var instructorExists = await _context.Instructors
                .AnyAsync(i => i.Id == dto.InstructorId);

            if (!instructorExists)
                return NotFound("Instructor not found");

            var course = new Course
            {
                Title = dto.Title,
                InstructorId = dto.InstructorId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok("Course created successfully");
        }

        // =========================
        // PUT: Update Course
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCourseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            // Check instructor exists
            var instructorExists = await _context.Instructors
                .AnyAsync(i => i.Id == dto.InstructorId);

            if (!instructorExists)
                return NotFound("Instructor not found");

            course.Title = dto.Title;
            course.InstructorId = dto.InstructorId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // =========================
        // DELETE: Remove Course
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}