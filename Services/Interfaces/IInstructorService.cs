using CourseManagementAPI.DTOs;

namespace CourseManagementAPI.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorDto>> GetAllAsync();
        Task<InstructorDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateInstructorDto dto, string userId);
        Task UpdateAsync(int id, UpdateInstructorDto dto);
        Task DeleteAsync(int id);
    }
}