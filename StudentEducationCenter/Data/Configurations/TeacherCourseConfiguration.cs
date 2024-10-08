using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class TeacherCourseConfiguration : IEntityTypeConfiguration<TeacherCourse>
    {
        public void Configure(EntityTypeBuilder<TeacherCourse> builder)
        {
            builder.HasKey(tc => new { tc.TeacherId, tc.CourseId });

            builder
               .HasOne(tc => tc.Teacher)
               .WithMany(t => t.TeacherCourses)
               .HasForeignKey(tc => tc.TeacherId)
               .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(tc => tc.Course)
                .WithMany(c => c.TeachersCourse)
                .HasForeignKey(tc => tc.CourseId)
                .OnDelete(DeleteBehavior.NoAction);
            // Teacher 1 - Математика, Информатика
            // Teacher 2 - Английски език, Немски език
            // Teacher 3 - Български език и литература, Немски език
            // Teacher 4 - Испански език, Френски език
            // Teacher 5 - Немски език, Испански език, Руски език

            List<TeacherCourse> list = new List<TeacherCourse>()
            {
                new TeacherCourse
                {
                    CourseId = 1,
                    TeacherId = 3,
                },
                new TeacherCourse
                {
                    CourseId = 2,
                    TeacherId = 3,
                },
                new TeacherCourse
                {
                    CourseId = 3,
                    TeacherId = 3,
                },
                new TeacherCourse
                {
                    CourseId = 3,
                    TeacherId = 5,
                },
                new TeacherCourse
                {
                    CourseId = 4,
                    TeacherId = 4,
                },
                new TeacherCourse
                {
                    CourseId = 5,
                    TeacherId = 1,
                },
            };

            builder.HasData(list);
        }
    }
}
