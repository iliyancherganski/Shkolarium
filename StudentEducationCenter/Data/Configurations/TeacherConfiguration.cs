using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            List<Teacher> teachers = new List<Teacher>()
            {
                new Teacher()
                {
                    Id = 1,
                    UserId = "4560faa9-53cd-4adc-8d65-4f7662cd30a7",
                },
                new Teacher()
                {
                    Id = 2,
                    UserId = "56b171cf-06d6-4943-97d1-63b5d914a348",
                },
                new Teacher()
                {
                    Id = 3,
                    UserId = "42949812-d05e-46a5-8de6-cb3520319734",
                },
                new Teacher()
                {
                    Id = 4,
                    UserId = "3d522657-3ef9-4118-9b07-e5cd9bfb8614",
                },
                new Teacher()
                {
                    Id = 5,
                    UserId = "ca1ee9ab-8832-464f-a197-7e64d8a4e8af",
                },
            };
            builder.HasData(teachers);
        }
    }
}
