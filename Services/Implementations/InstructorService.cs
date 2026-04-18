using Microsoft.EntityFrameworkCore;
using CourseManagementAPI;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services.Interfaces;
using CourseManagementAPI.Models;

namespace CourseManagementAPI.Services.Implementations
{
    public class InstructorService : IInstructorService
    {
        private readonly AppDbContext _context;

        public InstructorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InstructorDto>> GetAllAsync()
        {
            return await _context.Instructors
                .Include(i => i.Profile)
                .AsNoTracking()
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Profile != null ? i.Profile.Email : string.Empty
                })
                .ToListAsync();
        }

        public async Task<InstructorDto?> GetByIdAsync(int id)
        {
            return await _context.Instructors
                .Include(i => i.Profile)
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new InstructorDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Profile != null ? i.Profile.Email : string.Empty
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CreateInstructorDto dto, string userId)
        {
            var instructorExists =
                await _context.Instructors
                .AnyAsync(i => i.UserId == userId);

            if (instructorExists)
                throw new Exception("Instructor profile already exists");


            var studentExists =
                await _context.Students
                .AnyAsync(s => s.UserId == userId);

            if (studentExists)
                throw new Exception("Student cannot create instructor profile");


            var instructor =
                new Instructor
                {
                    Name = dto.Name,
                    UserId = userId
                };


            var profile =
                new InstructorProfile
                {
                    Email = dto.Email,
                    Instructor = instructor
                };


            _context.Instructors.Add(instructor);

            _context.InstructorProfiles.Add(profile);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateInstructorDto dto)
        {
            var instructor = await _context.Instructors
                .Include(i => i.Profile)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null) return;

            instructor.Name = dto.Name;

            if (instructor.Profile != null)
            {
                instructor.Profile.Email = dto.Email;
            }
            else
            {
                instructor.Profile = new InstructorProfile
                {
                    Email = dto.Email
                };
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return;

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
        }
    }
}