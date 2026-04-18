using Microsoft.EntityFrameworkCore;
using CourseManagementAPI;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services.Interfaces;
using CourseManagementAPI.Models;

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
                .Select(s => new StudentDto    //mapping 
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email
                })
                .ToListAsync();   //returns a list of students
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
                .FirstOrDefaultAsync();  //returns a single student or null if not found
        }

        public async Task<StudentDto?> GetByUserIdAsync(string userId)
        {
            return await _context.Students
                .Where(s => s.UserId == userId)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CreateStudentDto dto, string userId)
        {
            var studentExists =
                await _context.Students
                .AnyAsync(s => s.UserId == userId);

            if (studentExists)
                throw new Exception("Student profile already exists");


            var instructorExists =
                await _context.Instructors
                .AnyAsync(i => i.UserId == userId);

            if (instructorExists)
                throw new Exception("Instructor cannot create student profile");


            var student =
                new Student
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    UserId = userId
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