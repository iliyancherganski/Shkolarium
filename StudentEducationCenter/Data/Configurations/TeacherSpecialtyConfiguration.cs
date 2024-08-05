using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class TeacherSpecialtyConfiguration : IEntityTypeConfiguration<TeacherSpecialty>
    {
        public void Configure(EntityTypeBuilder<TeacherSpecialty> builder)
        {
            builder.HasKey(ts => new { ts.TeacherId, ts.SpecialtyId });

            List<TeacherSpecialty> teacherSpecialties = new List<TeacherSpecialty>()
            {
                // Specialties:
                // 1 - Математика
                // 2 - Информатика
                // 3 - Български език и литература
                // 4 - Английски език
                // 5 - Немски език
                // 6 - Испански език
                // 7 - Руски език
                // 8 - Френски език

                // Teacher 1 - Математика, Информатика
                new TeacherSpecialty()
                {
                    TeacherId = 1,
                    SpecialtyId = 1,
                },
                new TeacherSpecialty()
                {
                    TeacherId = 1,
                    SpecialtyId = 2,
                },

                // Teacher 2 - Английски език, Немски език
                new TeacherSpecialty()
                {
                    TeacherId = 2,
                    SpecialtyId = 4,
                },
                new TeacherSpecialty()
                {
                    TeacherId = 2,
                    SpecialtyId = 5,
                },

                // Teacher 3 - Български език и литература, Немски език
                new TeacherSpecialty()
                {
                    TeacherId = 3,
                    SpecialtyId = 3,
                },
                new TeacherSpecialty()
                {
                    TeacherId = 3,
                    SpecialtyId = 5,
                },

                // Teacher 4 - Испански език, Френски език
                new TeacherSpecialty()
                {
                    TeacherId = 4,
                    SpecialtyId = 6,
                },
                 new TeacherSpecialty()
                {
                    TeacherId = 4,
                    SpecialtyId = 8,
                },

                // Teacher 5 - Немски език, Испански език, Руски език
                new TeacherSpecialty()
                {
                    TeacherId = 5,
                    SpecialtyId = 5,
                },
                new TeacherSpecialty()
                {
                    TeacherId = 5,
                    SpecialtyId = 6,
                },
                new TeacherSpecialty()
                {
                    TeacherId = 5,
                    SpecialtyId = 7,
                },
            };
            builder.HasData(teacherSpecialties);
        }
    }
}
