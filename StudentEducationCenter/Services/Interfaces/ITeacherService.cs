using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Services.Interfaces
{
    public interface ITeacherService
    {
        public void AddNewTeacher(User user);
        public void EditTeacher(Teacher teacher);
    }
}
