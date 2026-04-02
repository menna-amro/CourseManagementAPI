using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManagementAPI.DTOs;

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
                .Include(i => i.Profile)
                .AsNoTracking()
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Profile != null ? i.Profile.Email : null
                })
                .ToListAsync();

            return Ok(instructors);
        }

        // GET: api/instructors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDto>> GetInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.Profile)
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Profile != null ? i.Profile.Email : null
                })
                .FirstOrDefaultAsync();

            if (instructor == null)
                return NotFound();

            return Ok(instructor);
        }

        // POST: api/instructors
        [HttpPost]
        public async Task<IActionResult> CreateInstructor(CreateInstructorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructor = new Instructor
            {
                Name = dto.Name,
                Profile = new InstructorProfile
                {
                    Email = dto.Email
                }
            };

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Instructor created successfully" });
        }

        // PUT: api/instructors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructor(int id, UpdateInstructorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var instructor = await _context.Instructors
                .Include(i => i.Profile)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
                return NotFound();

            instructor.Name = dto.Name;

            if (instructor.Profile != null)
            {
                instructor.Profile.Email = dto.Email;
            }
            else
            {
                instructor.Profile = new InstructorProfile
                {
                    Email = dto.Email,
                    InstructorId = instructor.Id
                };
            }

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