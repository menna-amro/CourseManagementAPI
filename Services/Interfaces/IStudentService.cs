using CourseManagementAPI.DTOs;

namespace CourseManagementAPI.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateStudentDto dto);
        Task UpdateAsync(int id, UpdateStudentDto dto);
        Task DeleteAsync(int id);
    }
}