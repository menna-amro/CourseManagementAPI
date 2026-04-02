using Microsoft.EntityFrameworkCore;


namespace CourseManagementAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorProfile> InstructorProfiles { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Primary Key (Many-to-Many via Enrollment)
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.StudentId, e.CourseId });

            // One-to-One: Instructor ↔ InstructorProfile
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Profile)
                .WithOne(p => p.Instructor)
                .HasForeignKey<InstructorProfile>(p => p.InstructorId);

            // One-to-Many: Instructor → Courses
            modelBuilder.Entity<Instructor>()
                .HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many: Student ↔ Course (via Enrollment)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}