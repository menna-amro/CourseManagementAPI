using Microsoft.EntityFrameworkCore;
using CourseManagementAPI;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Services.Interfaces;

namespace CourseManagementAPI.Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task EnrollAsync(CreateEnrollmentDto dto)
        {
            var exists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);

            if (exists)
                throw new Exception("Student already enrolled");

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EnrollmentDto>> GetStudentCoursesAsync(int studentId)
        {
            return await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .AsNoTracking()
                .Select(e => new EnrollmentDto
                {
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    StudentName = e.Student.Name,
                    CourseTitle = e.Course.Title
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<EnrollmentDto>> GetCourseStudentsAsync(int courseId)
        {
            return await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .AsNoTracking()
                .Select(e => new EnrollmentDto
                {
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    StudentName = e.Student.Name,
                    CourseTitle = e.Course.Title
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null) return;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}