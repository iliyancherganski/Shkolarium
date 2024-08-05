using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class SpecialityConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            List<Specialty> list = new List<Specialty>()
            {
                new Specialty()
                {
                    Id = 1,
                    Name = "Математика",
                },
                new Specialty()
                {
                    Id = 2,
                    Name = "Информатика",
                },
                new Specialty()
                {
                    Id = 3,
                    Name = "Български език и литература",
                },
                new Specialty()
                {
                    Id = 4,
                    Name = "Английски език",
                },
                new Specialty()
                {
                    Id = 5,
                    Name = "Немски език",
                },
                new Specialty()
                {
                    Id = 6,
                    Name = "Испански език",
                },
                new Specialty()
                {
                    Id = 7,
                    Name = "Руски език",
                },
                new Specialty()
                {
                    Id = 8,
                    Name = "Френски език",
                },
            };
            builder.HasData(list);
        }
    }
}
