using System.ComponentModel.DataAnnotations;

public class UpdateInstructorDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
}