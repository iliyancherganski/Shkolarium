using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            List<Parent> parents = new List<Parent>()
            {
                new Parent()
                {
                    Id = 1,
                    UserId = "f8ea9f65-046d-47ea-86c6-9e3c7b6b7d2c",
                },
                new Parent()
                {
                    Id = 2,
                    UserId = "e174d710-f687-4a09-876c-4a8690d393e5",
                },
                new Parent()
                {
                    Id = 3,
                    UserId = "80cae098-d8ab-42e2-bec2-0bb11d0a441e",
                },
                new Parent()
                {
                    Id = 4,
                    UserId = "b79f3723-9600-499a-b640-a368f7816b47",
                },
                new Parent()
                {
                    Id = 5,
                    UserId = "7e124ab7-6050-49d8-853f-e51897ff536f",
                },
            };
            builder.HasData(parents);
        }
    }
}
