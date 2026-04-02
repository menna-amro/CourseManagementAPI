public class InstructorProfile
{
    public int Id { get; set; }

    public string Bio { get; set; }

    public int InstructorId { get; set; }

    public Instructor Instructor { get; set; }
}