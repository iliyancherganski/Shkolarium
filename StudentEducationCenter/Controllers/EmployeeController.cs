using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.ViewModels.Employee;
using System.Globalization;

namespace StudentEducationCenter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : BaseController
    {
        public EmployeeController(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index(int? page,
            bool? sortByName = null,
            int cityId = 0,
            string? searchInput = null,
            string? message = null,
            bool isError = false)
        {
            List<Employee> employees = await _context.Employees
                .Include(x => x.User)
                .Include(x => x.Courses)
                .ThenInclude(x => x.AgeGroup)
                .Include(x => x.Courses)
                .ThenInclude(x => x.CourseType)
                .Include(x=>x.User)
                .ThenInclude(x=>x.City)
                .ToListAsync();
            if (message != null)
            {
                if (isError)
                {
                    ModelState.AddModelError(string.Empty, message);
                }
                else
                {
                    ViewBag.Message = message;
                }
            }
            await SortedCitiesInViewBag();

            ViewBag.SortByName = null;
            ViewBag.CityId = cityId;
            ViewBag.SearchInput = null;

            if (searchInput != null && !string.IsNullOrEmpty(searchInput))
            {
                string temp = searchInput.TrimEnd().ToUpper();

                List<Employee> tempEmployee = new List<Employee>();

                foreach (var employee in employees)
                {
                    if ($"{employee.User.FirstName.ToUpper()} {employee.User.LastName.ToUpper()}".Contains(temp)
                        || (employee.User.PhoneNumber != null && employee.User.PhoneNumber.Contains(temp)))
                    {
                        tempEmployee.Add(employee);
                    }
                }

                employees = tempEmployee;
                ViewBag.SearchInput = searchInput.TrimEnd();
            }
            else
            {
                // SORTING
                if (sortByName != null && sortByName == true)
                {
                    employees = employees.OrderBy(x => x.User.LastName).ThenBy(x => x.User.FirstName).ToList();
                    ViewBag.SortByName = true;
                }
                if (sortByName != null && sortByName == false)
                {
                    employees = employees.OrderBy(x => x.User.FirstName).ThenBy(x => x.User.LastName).ToList();
                    ViewBag.SortByName = false;
                }

                // FILTERING
                if (cityId != 0)
                {
                    employees = employees.Where(x => x.User.CityId == cityId).ToList();
                }
            }

            employees = Pagination(page, employees, 4);
            return View(employees);
        }

        public async Task<IActionResult> ReassignCourses(int employeeId)
        {
            var employee = await _context.Employees
                .Include(x => x.User)
                .Include(x => x.Courses)
                .ThenInclude(x => x.AgeGroup)
                .Include(x => x.Courses)
                .ThenInclude(x => x.CourseType)
                .Include(x => x.Courses)
                .ThenInclude(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee != null)
            {
                if (employee.Courses.Count <= 0)
                {
                    string messageТ2 = $"Всички отговорни служители на курсовете на служителя {employee.User.FirstName} {employee.User.LastName} са преразпределени и служителят е изтрит успешно.";
                    _context.Users.Remove(employee.User);
                    _context.Employees.Remove(employee);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { message = messageТ2, isError = false });
                }

                ViewBag.Employees = _context.Employees
                    .Include(x => x.User)
                    .Where(x => employee.Id != x.Id)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.User.FirstName + " " + x.User.LastName
                    });

                ReassignCoursesViewModel viewModel = new();
                var tempEmployeeId = await _context.Employees.FirstOrDefaultAsync(x => x.Id != employee.Id);
                if (tempEmployeeId == null)
                {
                    string messageT = $"Няма други служители, за които курсовете на текущия служител ({employee.User.FirstName} {employee.User.LastName}) ще могат да се пренапишат. Ако искате да изтриете този служител, трябва да изтриете първо всички негови курсове.";
                    return RedirectToAction("Index", new { message = messageT, isError = true });
                }

                foreach (var course in employee.Courses)
                {
                    string? courseTeachers = string.Join(", ", course.TeachersCourse.Select(x => x.Teacher.User.FirstName + " " + x.Teacher.User.LastName));
                    if (course.TeachersCourse.Count <= 0)
                    {
                        courseTeachers = null;
                    }
                    CourseEmployeeViewModel courseEmployeeViewModel = new CourseEmployeeViewModel()
                    {
                        EmployeeId = 0,
                        CourseId = course.Id,
                        CourseAgeGroupName = course.AgeGroup.Name,
                        CourseTypeName = course.CourseType.Name,
                        CourseTeachers = courseTeachers,
                        CourseStartDateToString = course.StartDate.ToString("MMMM dd, yyyy", new CultureInfo("bg-BG")),
                        CourseEndDateToString = course.EndDate.ToString("MMMM dd, yyyy", new CultureInfo("bg-BG"))
                    };

                    viewModel.coursesEmployees.Add(courseEmployeeViewModel);
                }
                viewModel.EmployeeId = employee.Id;
                viewModel.EmployeeFirstName = employee.User.FirstName;
                viewModel.EmployeeLastName = employee.User.LastName;

                return View(viewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ReassignCourses(ReassignCoursesViewModel model)
        {
            foreach (var courseEmployee in model.coursesEmployees)
            {
                var course = await _context.Courses
                    .Include(x => x.Employee)
                    .Include(x => x.CourseType)
                    .FirstOrDefaultAsync(x => x.Id == courseEmployee.CourseId);

                var employee = await _context.Employees
                    .Include(x => x.Courses)
                    .FirstOrDefaultAsync(x => x.Id == courseEmployee.EmployeeId);

                if (course == null)
                {
                    string messageT = $"Курсът {courseEmployee.CourseTypeName} ({courseEmployee.CourseStartDateToString} - {courseEmployee.CourseEndDateToString}) по някаква причина не е намерен и служителят {model.EmployeeFirstName} {model.EmployeeLastName} не е изтрит.";
                    return RedirectToAction("Index", new { message = messageT, isError = true });
                }
                if (employee == null)
                {
                    string messageT = $"Един или повече от служителите, за които курса/курсовете на служителя {model.EmployeeFirstName} {model.EmployeeLastName} трябваше да бъдат презаписани, не е или са намерени, затова служителят {model.EmployeeFirstName} {model.EmployeeLastName} не е изтрит.";
                    return RedirectToAction("Index", new { message = messageT, isError = true });
                }

                course.Employee = employee;
                course.EmployeeId = employee.Id;

                employee.Courses.Add(course);
            }

            var employeeOriginal = await _context.Employees
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == model.EmployeeId);
            if (employeeOriginal == null)
            {
                string messageT = $"Служителят {model.EmployeeFirstName} {model.EmployeeLastName} не бе намерен.";
                return RedirectToAction("Index", new { message = messageT, isError = true });
            }

            _context.Users.Remove(employeeOriginal.User);
            _context.Employees.Remove(employeeOriginal);

            await _context.SaveChangesAsync();

            string messageТ2 = $"Всички отговорни служители на курсовете на служителя {model.EmployeeFirstName} {model.EmployeeLastName} са преразпределени и служителят е изтрит успешно.";
            return RedirectToAction("Index", new { message = messageТ2, isError = false });
        }
    }
}
