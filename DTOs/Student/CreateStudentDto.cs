using System.ComponentModel.DataAnnotations;

public class CreateStudentDto
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}