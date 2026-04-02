public class InstructorProfile
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;
    public string? Address { get; set; }

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
}