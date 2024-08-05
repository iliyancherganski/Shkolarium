using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Data.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            List<City> list = new List<City>()
            {
                new City()
                {
                    Id = 1,
                    Name = "Бургас",
                },
                new City()
                {
                    Id = 2,
                    Name = "Варна",
                },
                new City()
                {
                    Id = 3,
                    Name = "Велико Търново",
                },
                new City()
                {
                    Id = 4,
                    Name = "Пловдив",
                },
                new City()
                {
                    Id = 5,
                    Name = "Русе",
                },
                new City()
                {
                    Id = 6,
                    Name = "София",
                },
                new City()
                {
                    Id = 7,
                    Name = "Стара Загора",
                },
                new City()
                {
                    Id = 8,
                    Name = "Благоевград",
                },
                new City()
                {
                    Id = 9,
                    Name = "Пазарджик",
                },
                new City()
                {
                    Id = 10,
                    Name = "Плевен",
                },
                new City()
                {
                    Id = 11,
                    Name = "Хасково",
                },
                new City()
                {
                    Id = 12,
                    Name = "Сливен",
                },
                new City()
                {
                    Id = 13,
                    Name = "Шумен",
                },
                new City()
                {
                    Id = 14,
                    Name = "Добрич",
                },
                new City()
                {
                    Id = 15,
                    Name = "Кърджали",
                },
                new City()
                {
                    Id = 16,
                    Name = "Враца",
                },
                new City()
                {
                    Id = 17,
                    Name = "Монтана",
                },
                new City()
                {
                    Id = 18,
                    Name = "Ловеч",
                },
                new City()
                {
                    Id = 19,
                    Name = "Перник",
                },
                new City()
                {
                    Id = 20,
                    Name = "Ямбол",
                },
                new City()
                {
                    Id = 21,
                    Name = "Кюстендил",
                },
                new City()
                {
                    Id = 22,
                    Name = "Търговище",
                },
                new City()
                {
                    Id = 23,
                    Name = "Разград",
                },
                new City()
                {
                    Id = 24,
                    Name = "Силистра",
                },
                new City()
                {
                    Id = 25,
                    Name = "Габрово",
                },
                new City()
                {
                    Id = 26,
                    Name = "Смолян",
                },
                new City()
                {
                    Id = 27,
                    Name = "Видин",
                },
            };

            builder.HasData(list);
        }
    }
}
