using CourseManagementAPI.DTOs;

namespace CourseManagementAPI.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task EnrollAsync(CreateEnrollmentDto dto);

        Task<IEnumerable<EnrollmentDto>> GetStudentCoursesAsync(int studentId);

        Task<IEnumerable<EnrollmentDto>> GetCourseStudentsAsync(int courseId);

        Task DeleteAsync(int studentId, int courseId);
    }
}