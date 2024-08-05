using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.Services.Interfaces;

namespace StudentEducationCenter.Services
{
    public class TeacherService : ITeacherService
    {
        public ApplicationDbContext context;
        public TeacherService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async void AddNewTeacher(User user)
        {
            await context.Users.AddAsync(user);
            Teacher teacher = new Teacher()
            {
                User = user,
                UserId = user.Id
            };
            await context.Teachers.AddAsync(teacher);
            await context.SaveChangesAsync();
        }

        public async void EditTeacher(Teacher teacher)
        {
            var t = await context.Teachers.FirstOrDefaultAsync(x => x.Id == teacher.Id);
            /*if (t != null)
            {
                t = 
            }*/
        }
    }
}
