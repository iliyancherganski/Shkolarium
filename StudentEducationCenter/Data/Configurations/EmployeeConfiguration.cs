using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee
                {
                    Id = 1,
                    UserId = "9789ec17-8fe9-47c2-9577-a2e86ebc85ea",
                },
                new Employee
                {
                    Id = 2,
                    UserId = "1d5a3070-fc27-4172-ad7c-8ef84fb88f7f",
                },
                new Employee
                {
                    Id = 3,
                    UserId = "0ab7ca6b-8108-4b96-a300-f998e93c1b83",
                },
            };
            builder.HasData(employees);
        }
    }
}
