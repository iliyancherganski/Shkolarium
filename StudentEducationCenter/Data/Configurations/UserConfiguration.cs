using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var hasher = new PasswordHasher<User>();
            var users = new List<User>();

            // ADMIN
            var user = new User()
            {
                Id = "aab83f21-bfce-46dc-b9b5-3fd6dd1d17f0",
                EmailConfirmed = true,
                FirstName = "Admin",
                MiddleName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.bg",
                NormalizedEmail = "ADMIN@ADMIN.BG",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "0812345678",
                CityId = 1,
                Address = "Необособен",
            };
            user.PasswordHash = hasher.HashPassword(user, "Admin123!");
            users.Add(user);

            // Teacher 1
            var teacher1 = new User()
            {
                Id = "4560faa9-53cd-4adc-8d65-4f7662cd30a7",
                EmailConfirmed = true,
                FirstName = "Николай",
                MiddleName = "Иванов",
                LastName = "Николов",
                Email = "teacher1@gmail.com",
                NormalizedEmail = "TEACHER1@GMAIL.COM",
                UserName = "teacher1@gmail.com",
                NormalizedUserName = "TEACHER1@GMAIL.COM",
                PhoneNumber = "0884672591",
                CityId = 3,
                Address = "ул. 'Никола Габровски' 15А",
            };
            teacher1.PasswordHash = hasher.HashPassword(teacher1, "Teacher123!");
            users.Add(teacher1);

            // Teacher 2
            var teacher2 = new User()
            {
                Id = "56b171cf-06d6-4943-97d1-63b5d914a348",
                EmailConfirmed = true,
                FirstName = "Преслав",
                MiddleName = "Николаев",
                LastName = "Калоянов",
                Email = "teacher2@gmail.com",
                NormalizedEmail = "TEACHER2@GMAIL.COM",
                UserName = "teacher2@gmail.com",
                NormalizedUserName = "TEACHER2@GMAIL.COM",
                PhoneNumber = "0888967530",
                CityId = 2,
                Address = "ул. 'Стоян Коледаров' 6",
            };
            teacher2.PasswordHash = hasher.HashPassword(teacher2, "Teacher123!");
            users.Add(teacher2);

            // Teacher 3
            var teacher3 = new User()
            {
                Id = "42949812-d05e-46a5-8de6-cb3520319734",
                EmailConfirmed = true,
                FirstName = "Мария",
                MiddleName = "Атанасова",
                LastName = "Димитрова",
                Email = "teacher3@gmail.com",
                NormalizedEmail = "TEACHER3@GMAIL.COM",
                UserName = "teacher3@gmail.com",
                NormalizedUserName = "TEACHER3@GMAIL.COM",
                PhoneNumber = "0899745867",
                CityId = 2,
                Address = "ул. 'Мусала' 12",
            };
            teacher3.PasswordHash = hasher.HashPassword(teacher3, "Teacher123!");
            users.Add(teacher3);

            // Teacher 4
            var teacher4 = new User()
            {
                Id = "3d522657-3ef9-4118-9b07-e5cd9bfb8614",
                EmailConfirmed = true,
                FirstName = "Виктор",
                MiddleName = "Петров",
                LastName = "Стефанов",
                Email = "teacher4@gmail.com",
                NormalizedEmail = "TEACHER4@GMAIL.COM",
                UserName = "teacher4@gmail.com",
                NormalizedUserName = "TEACHER4@GMAIL.COM",
                PhoneNumber = "0899745997",
                CityId = 4,
                Address = "ул. 'Васил Левски' 155А",
            };
            teacher4.PasswordHash = hasher.HashPassword(teacher4, "Teacher123!");
            users.Add(teacher4);

            // Teacher 5
            var teacher5 = new User()
            {
                Id = "ca1ee9ab-8832-464f-a197-7e64d8a4e8af",
                EmailConfirmed = true,
                FirstName = "Емил",
                MiddleName = "Давидов",
                LastName = "Долчинков",
                Email = "teacher5@gmail.com",
                NormalizedEmail = "TEACHER5@GMAIL.COM",
                UserName = "teacher5@gmail.com",
                NormalizedUserName = "TEACHER5@GMAIL.COM",
                PhoneNumber = "0899745000",
                CityId = 5,
                Address = "ул. 'Александър Малинов' 33",
            };
            teacher5.PasswordHash = hasher.HashPassword(teacher5, "Teacher123!");
            users.Add(teacher5);

            // Employee 1
            var employee1 = new User()
            {
                Id = "9789ec17-8fe9-47c2-9577-a2e86ebc85ea",
                EmailConfirmed = true,
                FirstName = "Димитър",
                MiddleName = "Спасов",
                LastName = "Иванов",
                Email = "employee1@gmail.com",
                NormalizedEmail = "EMPLOYEE1@GMAIL.COM",
                UserName = "employee1@gmail.com",
                NormalizedUserName = "EMPLOYEE1@GMAIL.COM",
                PhoneNumber = "0823167589",
                CityId = 1,
                Address = "ул. 'Петко Р. Славейков' 4Б",
            };
            employee1.PasswordHash = hasher.HashPassword(employee1, "Employee123!");
            users.Add(employee1);

            // Employee 2
            var employee2 = new User()
            {
                Id = "1d5a3070-fc27-4172-ad7c-8ef84fb88f7f",
                EmailConfirmed = true,
                FirstName = "Алиса",
                MiddleName = "Емилиянова",
                LastName = "Нешева",
                Email = "employee2@gmail.com",
                NormalizedEmail = "EMPLOYEE2@GMAIL.COM",
                UserName = "employee2@gmail.com",
                NormalizedUserName = "EMPLOYEE2@GMAIL.COM",
                PhoneNumber = "0877512844",
                CityId = 6,
                Address = "ул. 'Плиска' 22",
            };
            employee2.PasswordHash = hasher.HashPassword(employee2, "Employee123!");
            users.Add(employee2);

            // Employee 3
            var employee3 = new User()
            {
                Id = "0ab7ca6b-8108-4b96-a300-f998e93c1b83",
                EmailConfirmed = true,
                FirstName = "Петър",
                MiddleName = "Валентинов",
                LastName = "Нешев",
                Email = "employee3@gmail.com",
                NormalizedEmail = "EMPLOYEE3@GMAIL.COM",
                UserName = "employee3@gmail.com",
                NormalizedUserName = "EMPLOYEE3@GMAIL.COM",
                PhoneNumber = "0877512889",
                CityId = 4,
                Address = "ул. 'Преслав' 10",
            };
            employee3.PasswordHash = hasher.HashPassword(employee3, "Employee123!");
            users.Add(employee3);

            // Parent 1
            var parent1 = new User()
            {
                Id = "f8ea9f65-046d-47ea-86c6-9e3c7b6b7d2c",
                EmailConfirmed = true,
                FirstName = "Петър",
                MiddleName = "Иванов",
                LastName = "Петров",
                Email = "parent1@gmail.com",
                NormalizedEmail = "PARENT1@GMAIL.COM",
                UserName = "parent1@gmail.com",
                NormalizedUserName = "PARENT1@GMAIL.COM",
                PhoneNumber = "0859989615",
                CityId = 3,
                Address = "ул. 'Опълченска' 3А",
            };
            parent1.PasswordHash = hasher.HashPassword(parent1, "Parent123!");
            users.Add(parent1);

            // Parent 2
            var parent2 = new User()
            {
                Id = "e174d710-f687-4a09-876c-4a8690d393e5",
                EmailConfirmed = true,
                FirstName = "Александър",
                MiddleName = "Иванов",
                LastName = "Стратиев",
                Email = "parent2@gmail.com",
                NormalizedEmail = "PARENT2@GMAIL.COM",
                UserName = "parent2@gmail.com",
                NormalizedUserName = "PARENT2@GMAIL.COM",
                PhoneNumber = "0854700615",
                CityId = 1,
                Address = "ул. 'Стоян Коледаров' 3А",
            };
            parent2.PasswordHash = hasher.HashPassword(parent2, "Parent123!");
            users.Add(parent2);

            // Parent 3
            var parent3 = new User()
            {
                Id = "80cae098-d8ab-42e2-bec2-0bb11d0a441e",
                EmailConfirmed = true,
                FirstName = "Виктория",
                MiddleName = "Николаева",
                LastName = "Ангелова",
                Email = "parent3@gmail.com",
                NormalizedEmail = "PARENT3@GMAIL.COM",
                UserName = "parent3@gmail.com",
                NormalizedUserName = "PARENT3@GMAIL.COM",
                PhoneNumber = "0854789615",
                CityId = 6,
                Address = "ул. 'Петя Дубарова' 16Б",
            };
            parent3.PasswordHash = hasher.HashPassword(parent3, "Parent123!");
            users.Add(parent3);

            // Parent 4
            var parent4 = new User()
            {
                Id = "b79f3723-9600-499a-b640-a368f7816b47",
                EmailConfirmed = true,
                FirstName = "Девора",
                MiddleName = "Кирилова",
                LastName = "Кирилова",
                Email = "parent4@gmail.com",
                NormalizedEmail = "PARENT4@GMAIL.COM",
                UserName = "parent4@gmail.com",
                NormalizedUserName = "PARENT4@GMAIL.COM",
                PhoneNumber = "0865789488",
                CityId = 4,
                Address = "ул. 'Освобождение' 3Б",
            };
            parent4.PasswordHash = hasher.HashPassword(parent4, "Parent123!");
            users.Add(parent4);

            // Parent 5
            var parent5 = new User()
            {
                Id = "7e124ab7-6050-49d8-853f-e51897ff536f",
                EmailConfirmed = true,
                FirstName = "Даниела",
                MiddleName = "Сергеева",
                LastName = "Петкова",
                Email = "parent5@gmail.com",
                NormalizedEmail = "PARENT5@GMAIL.COM",
                UserName = "parent5@gmail.com",
                NormalizedUserName = "PARENT5@GMAIL.COM",
                PhoneNumber = "0813528746",
                CityId = 4,
                Address = "ул. 'Георги Бенковски' 25",
            };
            parent5.PasswordHash = hasher.HashPassword(parent5, "Parent123!");
            users.Add(parent5);

            builder.HasData(users);
        }
    }
}
