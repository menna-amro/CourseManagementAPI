using System.ComponentModel.DataAnnotations;

public class CreateInstructorDto
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
}