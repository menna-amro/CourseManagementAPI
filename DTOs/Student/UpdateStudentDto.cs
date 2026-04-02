using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs
{
    public class UpdateStudentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}