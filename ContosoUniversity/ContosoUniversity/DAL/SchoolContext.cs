using ContosoUniversity.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)   //For the many-to-many relationship between the Instructor and Course entities, 
                                                                               //the code specifies the table and column names for the join table. Code First 
                                                                               //can configure the many-to-many relationship for you without this code, but if 
                                                                               //you don't call it, you will get default names such as InstructorInstructorID 
                                                                               //for the InstructorID column.
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Course>()
            .HasMany(c => c.Instructors).WithMany(i => i.Courses)
            .Map(t => t.MapLeftKey("CourseID")
            .MapRightKey("InstructorID")
            .ToTable("CourseInstructor"));
        }
    }
}

//pg. 148, Contoso Unversity; replaces code below


//using System;
//using ContosoUniversity.Models;
//using System.Data.Entity;
//using System.Data.Entity.ModelConfiguration.Conventions;
//namespace ContosoUniversity.DAL
//{
//    public class SchoolContext : DbContext
//    {
//        public SchoolContext() : base("SchoolContext")
//        {
//        }
//        public DbSet<Student> Students { get; set; }
//        public DbSet<Enrollment> Enrollments { get; set; }
//        public DbSet<Course> Courses { get; set; }
//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
    
    
//     modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
//        }
//    }
//}

