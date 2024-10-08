using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;
using System.Collections.Generic;

namespace StudentEducationCenter.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // AgeGroudIds:
            // 1 - 5 клас
            // 2 - 6 клас
            // 3 - 7 клас
            // 4 - 5-7 клас

            // CourseTypeIds:
            // 1 - Математика
            // 2 - Български език и литература
            // 3 - Програмиране със C#
            // 4 - Английски език
            // 5 - Немски език
            // 6 - Френски език

            // Teachers:
            // 1 - Математика
            // 2 - Български език и литература
            // 3 - Програмиране със C#
            // 4 - Английски език
            // 5 - Немски език
            // 6 - Френски език

            List<Course> list = new List<Course>()
            {
                new Course
                {
                    Id = 1,
                    EmployeeId = 1,
                    AgeGroupId = 1, 
                    CourseTypeId = 2,
                    StartDate = new DateTime(2023, 10, 2),
                    EndDate = new DateTime(2024, 1, 15),
                    Price = 200,
                },

                new Course
                {
                    Id = 2,
                    EmployeeId = 1,
                    AgeGroupId = 1,
                    CourseTypeId = 2,
                    StartDate = new DateTime(2024, 1, 2),
                    EndDate = new DateTime(2024, 4, 15),
                    Price = 250,
                },

                new Course
                {
                    Id = 3,
                    EmployeeId = 2,
                    AgeGroupId = 4,
                    CourseTypeId = 5,
                    StartDate = new DateTime(2024, 1, 2),
                    EndDate = new DateTime(2024, 6, 15),
                    Price = 630,
                },

                new Course
                {
                    Id = 4,
                    EmployeeId = 3,
                    AgeGroupId = 1,
                    CourseTypeId = 6,
                    StartDate = new DateTime(2024, 10, 11),
                    EndDate = new DateTime(2025, 5, 25),
                    Price = 590,
                },

                new Course
                {
                    Id = 5,
                    EmployeeId = 3,
                    AgeGroupId = 3,
                    CourseTypeId = 3,
                    StartDate = new DateTime(2023, 11, 10),
                    EndDate = new DateTime(2024, 6, 29),
                    Price = 990,
                },
            };

            builder.HasData(list);
        }
    }
}
