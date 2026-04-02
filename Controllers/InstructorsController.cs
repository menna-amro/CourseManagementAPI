using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManagementAPI.DTOs.Instructor;

namespace CourseManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InstructorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/instructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorDto>>> GetInstructors()
        {
            var instructors = await _context.Instructors
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync();

            return Ok(instructors);
        }

        // GET: api/instructors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDto>> GetInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Where(i => i.Id == id)
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .FirstOrDefaultAsync();

            if (instructor == null)
                return NotFound();

            return Ok(instructor);
        }

        // POST: api/instructors
        [HttpPost]
        public async Task<ActionResult<InstructorDto>> CreateInstructor(CreateInstructorDto dto)
        {
            var instructor = new Instructor
            {
                Name = dto.Name
            };

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            var result = new InstructorDto
            {
                Id = instructor.Id,
                Name = instructor.Name
            };

            return CreatedAtAction(nameof(GetInstructor), new { id = instructor.Id }, result);
        }

        // PUT: api/instructors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructor(int id, UpdateInstructorDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
                return NotFound();

            instructor.Name = dto.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/instructors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
                return NotFound();

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}