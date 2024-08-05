using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Controllers
{
    [Authorize]
    public class TeacherController : BaseController
    {

        public TeacherController(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index(int? page,
            bool? sortByName = null,
            string? searchInput = null,
            int cityId = 0,
            int specialtyId = 0)
        {
            List<Teacher> teachers = await _context.Teachers
                .Include(x => x.User)
                .Include(x => x.TeacherSpecialties)
                .ThenInclude(x => x.Specialty)
                .Include(x => x.User)
                .ThenInclude(x => x.City)
                .ToListAsync();
            await PutInViewBag();

            ViewBag.SortByName = null;
            ViewBag.CityId = cityId;
            ViewBag.SearchInput = null;
            ViewBag.SpecialtyId = specialtyId;

            if (searchInput != null && !string.IsNullOrEmpty(searchInput))
            {
                string temp = searchInput.TrimEnd().ToUpper();

                List<Teacher> tempTeacher = new List<Teacher>();

                foreach (var teacher in teachers)
                {
                    if ($"{teacher.User.FirstName.ToUpper()} {teacher.User.LastName.ToUpper()}".Contains(temp)
                        || (teacher.User.PhoneNumber != null && teacher.User.PhoneNumber.Contains(temp)))
                    {
                        tempTeacher.Add(teacher);
                    }
                }

                teachers = tempTeacher;
                ViewBag.SearchInput = searchInput.TrimEnd();
            }
            else
            {
                // SORTING
                if (sortByName != null && sortByName == true)
                {
                    teachers = teachers.OrderBy(x => x.User.LastName).ThenBy(x => x.User.FirstName).ToList();
                    ViewBag.SortByName = true;
                }
                if (sortByName != null && sortByName == false)
                {
                    teachers = teachers.OrderBy(x => x.User.FirstName).ThenBy(x => x.User.LastName).ToList();
                    ViewBag.SortByName = false;
                }

                // FILTERING
                if (cityId != 0)
                {
                    teachers = teachers.Where(x => x.User.CityId == cityId).ToList();
                }
                if (specialtyId != 0)
                {
                    teachers = teachers.Where(x => x.TeacherSpecialties.Any(x => x.SpecialtyId == specialtyId)).ToList();
                }
            }
            
            teachers = Pagination(page, teachers, 4);

            return View(teachers);
        }

        public async Task<IActionResult> Info(int teacherId)
        {
            var teacher = await _context.Teachers
                .Include(x => x.User)
                .Include(x => x.TeacherCourses)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.AgeGroup)
                .Include(x => x.TeacherCourses)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.CourseRequests)
                .Include(x => x.TeacherCourses)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.CourseType)
                .Include(x => x.TeacherCourses)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .Include(x => x.TeacherSpecialties)
                .ThenInclude(x => x.Specialty)
                .FirstOrDefaultAsync(x => x.Id == teacherId);

            if (teacher != null)
            {
                return View(teacher);
            }
            return RedirectToAction(nameof(Index), "Teacher");
        }

        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Delete(int teacherId)
        {
            var teacher = await _context.Teachers
                .Include(x => x.User)
                .Include(x => x.TeacherCourses)
                .ThenInclude(x => x.Course)
                .Include(x => x.User)
                .Include(x => x.TeacherSpecialties)
                .ThenInclude(x => x.Specialty)
                .ThenInclude(x => x.TeachersSpecialty)
                .FirstOrDefaultAsync(x => x.Id == teacherId);

            if (teacher != null)
            {
                if (User.IsInRole("Admin") ||
                    (User.IsInRole("Teacher") && User.Identity?.Name == teacher.User.Email))
                {
                    var teacherCourses = teacher.TeacherCourses.ToList();
                    foreach (var tc in teacherCourses)
                    {
                        tc.Course.TeachersCourse.Remove(tc);
                        teacher.TeacherCourses.Remove(tc);
                        _context.TeachersCourses.Remove(tc);
                    }

                    var teacherSpecialties = teacher.TeacherSpecialties.ToList();
                    foreach (var ts in teacherSpecialties)
                    {
                        ts.Specialty.TeachersSpecialty.Remove(ts);
                        teacher.TeacherSpecialties.Remove(ts);
                        _context.TeachersSpecialties.Remove(ts);
                    }

                    _context.Users.Remove(teacher.User);
                    _context.Teachers.Remove(teacher);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index), "Teacher");
        }
        private async Task PutInViewBag()
        {
            await SortedCitiesInViewBag();

            ViewBag.Specialties = _context.Specialties.OrderBy(x => x.Name).Select(x => new SelectListItem
			{
				Value = x.Id.ToString(),
				Text = x.Name
			}).ToList();
		}
    }
}
