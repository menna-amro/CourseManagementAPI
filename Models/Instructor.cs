public class Instructor
{
    public int Id { get; set; }

    public string Name { get; set; }

    public InstructorProfile Profile { get; set; }

    public ICollection<Course> Courses { get; set; }
}