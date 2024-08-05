using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Enums;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.ViewModels.Course;

namespace StudentEducationCenter.Controllers
{
    [Authorize(Roles = "Employee,Admin")]
    public class CourseController : BaseController
    {
        public CourseController(ApplicationDbContext context) : base(context)
        {
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? page,
            bool sortByName = false,
            bool sortByStartDate = false,
            int teacherId = 0,
            int ageGroupId = 0,
            bool myCoursesOpened = false,
            int[]? myCoursesIds = null)
        {
            List<Course> courses = await _context.Courses
                .Include(x => x.CourseType)
                .Include(x => x.AgeGroup)
                .Include(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.User)
                .Include(x => x.CourseRequests)
                .ToListAsync();
            ViewBag.SortByName = false;
            ViewBag.SortByStartDate = false;
            ViewBag.AgeGroupId = ageGroupId;
            ViewBag.TeacherId = teacherId;

            PutInViewBag();

            // SORTING 
            if (sortByName)
            {
                courses = courses.OrderBy(x => x.CourseType.Name).ToList();
                ViewBag.SortByName = true;
            }
            if (sortByStartDate)
            {
                courses = courses.OrderBy(x => x.StartDate).ToList();
                ViewBag.SortByStartDate = true;
            }

            // FILTERING 
            if (ageGroupId != 0)
            {
                courses = courses.Where(x => x.AgeGroupId == ageGroupId).ToList();
            }
            if (teacherId != 0)
            {
                courses = courses.Where(x => x.TeachersCourse.Any(x => x.TeacherId == teacherId)).ToList();
            }

            if (myCoursesIds != null && myCoursesOpened)
            {
                courses = courses
                .Where(x => myCoursesIds.Contains(x.Id))
                .ToList();
            }
            else
            {
                courses = Pagination(page, courses, 8);
            }
            return View(courses);
        }

        public IActionResult Add()
        {
            PutInViewBag();
            AddCourseViewModel model = new();
            if (User.IsInRole("Employee"))
            {
                var empoyee = _context.Employees.FirstOrDefault(x => x.User.Id == GetUserId());
                if (empoyee != null)
                {
                    model.EmployeeId = empoyee.Id;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TeacherIds.Count <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Трябва да бъде маркиран поне един учител");
                    PutInViewBag();
                    return View(model);
                }

                var ageGroup = await _context.AgeGroups.FirstOrDefaultAsync(x => x.Id == model.AgeGroupId);
                if (ageGroup == null)
                {
                    ModelState.AddModelError(string.Empty, "Невалидна възрастова група");
                    PutInViewBag();
                    return View(model);
                }

                Employee? employee = null;
                if (User.IsInRole("Employee"))
                {
                    employee = await _context.Employees.FirstOrDefaultAsync(x => x.User.Id == GetUserId());
                }
                else if (User.IsInRole("Admin"))
                {
                    employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == model.EmployeeId);
                }
                if (employee == null)
                {
                    ModelState.AddModelError(string.Empty, "Невалиден служител");
                    PutInViewBag();
                    return View(model);
                }

                var courseType = await _context.CourseTypes.FirstOrDefaultAsync(x => x.Id == model.CourseTypeId);
                if (courseType == null)
                {
                    ModelState.AddModelError(string.Empty, "Невалиден тип курс");
                    PutInViewBag();
                    return View(model);
                }

                if (model.StartDate > model.EndDate)
                {
                    var tempDate = model.StartDate;
                    model.StartDate = model.EndDate;
                    model.EndDate = tempDate;
                }

                Course course = new Course()
                {
                    AgeGroupId = model.AgeGroupId,
                    AgeGroup = ageGroup,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Price = model.Price,
                    Employee = employee,
                    EmployeeId = employee.Id,
                    CourseType = courseType,
                    CourseTypeId = model.CourseTypeId,
                };
                await _context.Courses.AddAsync(course);

                var teachersCourse = new List<TeacherCourse>();
                foreach (var teacherId in model.TeacherIds)
                {
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == teacherId);
                    if (teacher != null)
                    {
                        var tc = new TeacherCourse()
                        {
                            Teacher = teacher,
                            TeacherId = teacherId,
                            Course = course,
                            CourseId = course.Id
                        };
                        await _context.TeachersCourses.AddAsync(tc);

                        teachersCourse.Add(tc);
                    }
                }
                course.TeachersCourse = teachersCourse;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Невалидни данни!");
            PutInViewBag();
            return View(model);
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int courseId)
        {
            PutInViewBag();
            var course = await _context.Courses
                .Include(x => x.CourseType)
                .Include(x => x.AgeGroup)
                .Include(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
            {
                return RedirectToAction("Index");
            }
            if (User.IsInRole("Admin") ||
                (User.IsInRole("Employee")
                && course.Employee.User.Id == GetUserId()))
            {
                var model = new AddCourseViewModel()
                {
                    Id = course.Id,
                    AgeGroupId = course.AgeGroupId,
                    CourseTypeId = course.CourseTypeId,
                    EmployeeId = course.EmployeeId,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate,
                    Price = course.Price,
                    TeacherIds = course.TeachersCourse.Select(x => x.TeacherId).ToList(),
                };

                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var course = await _context.Courses
                .Include(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            if (course == null)
            {
                ModelState.AddModelError(string.Empty, "Невалиден курс");
                return View(model);
            }

            var ageGroup = await _context.AgeGroups.FirstOrDefaultAsync(x => x.Id == model.AgeGroupId);
            if (ageGroup != null)
            {
                ModelState.AddModelError(string.Empty, "Невалидна възрастова група");
                course.AgeGroup = ageGroup;
                course.AgeGroupId = ageGroup.Id;
            }

            Employee? employee = null;
            employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == model.EmployeeId);
            if (employee != null)
            {
                ModelState.AddModelError(string.Empty, "Невалиден служител");
                course.Employee = employee;
                course.EmployeeId = employee.Id;
            }

            var courseType = await _context.CourseTypes.FirstOrDefaultAsync(x => x.Id == model.CourseTypeId);
            if (courseType != null)
            {
                ModelState.AddModelError(string.Empty, "Невалиден тип курс");
                course.CourseType = courseType;
                course.CourseTypeId = courseType.Id;
            }

            if (model.StartDate > model.EndDate)
            {
                (model.EndDate, model.StartDate) = (model.StartDate, model.EndDate);
            }
            course.StartDate = model.StartDate;
            course.EndDate = model.EndDate;
            course.Price = model.Price;

            var teacherIds = model.TeacherIds;

            /*if (model.TeacherIds.Count <= 0)
            {
                ModelState.AddModelError(string.Empty, "Трябва да бъде маркиран поне един учител");
                PutInViewBag();
                return View(model);
            }*/

            var tcs = course.TeachersCourse.ToList();
            course.TeachersCourse = new List<TeacherCourse>();
            foreach (var tc in tcs)
            {
                var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == tc.TeacherId);
                if (teacher != null)
                {
                    teacher.TeacherCourses.Remove(tc);
                    _context.TeachersCourses.Remove(tc);
                }
                else
                    ModelState.AddModelError(string.Empty, "Невалиден учител");

            }
            foreach (var teacherId in model.TeacherIds)
            {
                var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == teacherId);
                if (teacher != null)
                {
                    TeacherCourse tc = new()
                    {
                        Course = course,
                        CourseId = course.Id,
                        Teacher = teacher,
                        TeacherId = teacherId
                    };
                    _context.TeachersCourses.Add(tc);
                    teacher.TeacherCourses.Add(tc);
                    course.TeachersCourse.Add(tc);
                }
                else
                    ModelState.AddModelError(string.Empty, "Невалиден учител");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int courseId)
        {
            var course = await _context.Courses
               .Include(x => x.TeachersCourse)
               .ThenInclude(x => x.Teacher)
               .Include(x => x.CourseRequests)
               .ThenInclude(x => x.Child)
               .FirstOrDefaultAsync(x => x.Id == courseId);
            if (course != null)
            {
                var tcs = course.TeachersCourse.ToList();
                foreach (var tc in tcs)
                {
                    var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == tc.TeacherId);
                    if (teacher != null)
                    {
                        teacher.TeacherCourses.Remove(tc);
                        _context.TeachersCourses.Remove(tc);
                    }
                }

                var courseRequests = course.CourseRequests.ToList();
                foreach (var cr in courseRequests)
                {
                    var child = cr.Child;
                    child.CourseRequests.Remove(cr);
                    course.CourseRequests.Remove(cr);
                    _context.CourseRequests.Remove(cr);
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Authorize(Roles = "Admin, Parent, Employee, Teacher")]
        public async Task<IActionResult> Info(int courseId)
        {
            var course = await _context.Courses
               .Include(x => x.CourseType)
               .Include(x => x.AgeGroup)
               .Include(x => x.Employee)
               .ThenInclude(x => x.User)
               .Include(x => x.TeachersCourse)
               .ThenInclude(x => x.Teacher)
               .ThenInclude(x => x.User)
               .Include(x => x.CourseRequests)
               .ThenInclude(x => x.Child)
               .ThenInclude(x => x.Parent)
               .ThenInclude(x => x.User)
               .FirstOrDefaultAsync(x => x.Id == courseId);
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> RemoveFromCourse(int childId, int courseId)
        {
            var courseRequest = await _context.CourseRequests
               .Include(x => x.Course)
               .Include(x => x.Child)
               .FirstOrDefaultAsync(x => x.CourseId == courseId && x.ChildId == childId);
            if (courseRequest != null)
            {
                courseRequest.Child.CourseRequests.Remove(courseRequest);
                courseRequest.Course.CourseRequests.Remove(courseRequest);
                _context.CourseRequests.Remove(courseRequest);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Info", new { courseId = courseId });
        }
        public async Task<IActionResult> Requests()
        {
            var course = await _context.Courses
                .Where(x => x.CourseRequests.Count(x => x.Status == RequestStatus.Pending) > 0)
                .Include(x => x.CourseType)
                .Include(x => x.AgeGroup)
                .Include(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.User)
                .Include(x => x.CourseRequests.Where(x => x.Status == RequestStatus.Pending))
                .ThenInclude(x => x.Child)
                .ThenInclude(x => x.Parent)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.City)
                .ToListAsync();

            if (User.IsInRole("Employee"))
            {
                course = course.Where(x => x.Employee.User.Id == GetUserId()).ToList();
            }
            return View(course);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MyCourses()
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == GetUserId());

            if (user != null && (User.IsInRole("Teacher") || User.IsInRole("Employee")))
            {
                List<Course>? courses = await _context.Courses
                .Include(x => x.CourseType)
                .Include(x => x.AgeGroup)
                .Include(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.User)
                .Include(x => x.CourseRequests)
                .Where(x => x.Employee.User == user || x.TeachersCourse.Select(x => x.Teacher.User).Contains(user))
                .ToListAsync();
                return RedirectToAction("Index", new { myCoursesOpened = true, myCoursesIds = courses.Select(x => x.Id).ToArray() });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Requests(int requestId, RequestStatus status)
        {
            var request = await _context.CourseRequests.FirstOrDefaultAsync(x => x.Id == requestId);
            if (request != null)
            {
                request.Status = status;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Requests");
        }

        private void PutInViewBag()
        {
            ViewBag.AgeGroups = _context.AgeGroups.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            ViewBag.CourseTypes = _context.CourseTypes.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            ViewBag.Employees = _context.Employees.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.User.FirstName} {x.User.LastName}"
            });

            ViewBag.Teachers = _context.Teachers
                .Include(x => x.User)
                .OrderBy(x => x.User.FirstName)
                .ThenBy(x => x.User.LastName)
                .Select(x => new
                {
                    TeacherId = x.Id,
                    TeacherName = $"{x.User.FirstName} {x.User.LastName} - {x.User.City.Name.ToUpper()} - {string.Join(", ", x.TeacherSpecialties.Select(x => x.Specialty.Name))}"
                });
        }
    }
}
