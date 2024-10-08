using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class ChildConfiguration : IEntityTypeConfiguration<Child>
    {
        public void Configure(EntityTypeBuilder<Child> builder)
        {
            List<Child> list = new List<Child>()
            {
                new Child
                {
                    Id = 1,
                    FirstName = "Иван",
                    LastName = "Петров",
                    PhoneNumber = "0895741239",
                    ParentId = 1,
                },

                new Child
                {
                    Id = 2,
                    FirstName = "Валентина",
                    LastName = "Стратева",
                    PhoneNumber = "0895741000",
                    ParentId = 2,
                },

                new Child
                {
                    Id = 3,
                    FirstName = "Мария",
                    LastName = "Стратева",
                    PhoneNumber = "0895987987",
                    ParentId = 2,
                },

                new Child
                {
                    Id = 4,
                    FirstName = "Ребека",
                    LastName = "Стратева",
                    PhoneNumber = "0895987112",
                    ParentId = 2,
                },

                new Child
                {
                    Id = 5,
                    FirstName = "Димитър",
                    LastName = "Кирилов",
                    PhoneNumber = "0895965532",
                    ParentId = 4,
                },

                new Child
                {
                    Id = 6,
                    FirstName = "Емил",
                    LastName = "Кирилов",
                    PhoneNumber = "0887951146",
                    ParentId = 4,
                },

                new Child
                {
                    Id = 7,
                    FirstName = "Матилда",
                    LastName = "Кирилова",
                    PhoneNumber = "0899915858",
                    ParentId = 4,
                },

                new Child
                {
                    Id = 8,
                    FirstName = "Мартина",
                    LastName = "Петкова",
                    PhoneNumber = "0899915333",
                    ParentId = 5,
                },

                new Child
                {
                    Id = 9,
                    FirstName = "Петко",
                    LastName = "Петков",
                    PhoneNumber = "0897332255",
                    ParentId = 5,
                },
            };

            builder.HasData(list);
        }
    }
}
