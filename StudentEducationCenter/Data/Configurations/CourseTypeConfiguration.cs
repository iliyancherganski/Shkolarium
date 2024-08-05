using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class CourseTypeConfiguration : IEntityTypeConfiguration<CourseType>
    {
        public void Configure(EntityTypeBuilder<CourseType> builder)
        {
            List<CourseType> courseTypes = new List<CourseType>()
            {
                new CourseType()
                {
                    Id = 1,
                    Name = "Математика"
                },
                new CourseType()
                {
                    Id = 2,
                    Name = "Български език и литература"
                },
                new CourseType()
                {
                    Id = 3,
                    Name = "Програмиране със C#"
                },
                new CourseType()
                {
                    Id = 4,
                    Name = "Английски език"
                },
                new CourseType()
                {
                    Id = 5,
                    Name = "Немски език"
                },
                new CourseType()
                {
                    Id = 6,
                    Name = "Френски език"
                },
            };
            builder.HasData(courseTypes);
        }
    }
}
