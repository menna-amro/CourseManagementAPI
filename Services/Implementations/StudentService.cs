using Microsoft.EntityFrameworkCore;
using CourseManagementAPI;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services.Interfaces;

namespace CourseManagementAPI.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            return await _context.Students
                .AsNoTracking()
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email
                })
                .ToListAsync();
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            return await _context.Students
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CreateStudentDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateStudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return;

            student.Name = dto.Name;
            student.Email = dto.Email;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}