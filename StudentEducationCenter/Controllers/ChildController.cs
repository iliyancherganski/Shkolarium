using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.ViewModels.Child;
using System.Globalization;

namespace StudentEducationCenter.Controllers
{
    [Authorize]
    public class ChildController : BaseController
    {
        public ChildController(ApplicationDbContext context) : base(context)
        {
        }

        [Authorize(Roles = "Admin,Parent")]
        public async Task<IActionResult> Index(
            int? page, 
            string? message = null, 
            bool? sortByName = null, 
            int cityId = 0, 
            string? searchInput = null)
        {
            List<Child> children = await _context.Children
                .Include(x => x.Parent)
                .ThenInclude(x => x.User)
                .ThenInclude(x => x.City)
                .Include(x => x.CourseRequests)
                .ToListAsync();

            ViewBag.Message = message;
            ViewBag.SortByName = null;
            ViewBag.CityId = cityId;
            ViewBag.SearchInput = null;

            await SortedCitiesInViewBag();

            if (User.IsInRole("Parent") || User.IsInRole("Admin"))
            {
                if (User.IsInRole("Parent"))
                {
                    var parent = await _context.Parents.FirstOrDefaultAsync(x => x.User.Id == GetUserId());
                    if (parent == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    children = children.Where(x => x.ParentId == parent.Id).ToList();
                }

                if (searchInput != null && !string.IsNullOrEmpty(searchInput))
                {
                    string temp = searchInput.TrimEnd().ToUpper();

                    List<Child> tempChildren = new List<Child>();

                    foreach (var child in children)
                    {
                        if ($"{child.FirstName.ToUpper()} {child.LastName.ToUpper()}".Contains(temp)
                            || (child.Parent.User.PhoneNumber != null && child.Parent.User.PhoneNumber.Contains(temp))
                            || (child.PhoneNumber != null && child.PhoneNumber.Contains(temp)))
                        {
                            tempChildren.Add(child);
                        }
                    }

                    children = tempChildren;
                    ViewBag.SearchInput = searchInput.TrimEnd();
                }
                else
                {
                    // SORTING
                    if (sortByName != null && sortByName == true)
                    {
                        children = children.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
                        ViewBag.SortByName = true;
                    }
                    if (sortByName != null && sortByName == false)
                    {
                        children = children.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
                        ViewBag.SortByName = false;
                    }

                    // FILTERING
                    if (cityId != 0)
                    {
                        children = children.Where(x => x.Parent.User.CityId == cityId).ToList();
                    }
                }

                children = Pagination(page, children, 8);

                return View(children);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin,Parent")]
        public async Task<IActionResult> Add()
        {
            await ParentsInViewBag();
            return View(new AddChildViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Parent, Admin")]
        public async Task<IActionResult> Add(AddChildViewModel model, 
            bool signInToCourse)
        {
            if (ModelState.IsValid
                && (User.IsInRole("Parent") || User.IsInRole("Admin")))
            {
                var parent = new Parent();
                if (User.IsInRole("Parent"))
                {
                    var userId = GetUserId();
                    parent = await _context.Parents.FirstOrDefaultAsync(x => x.User.Id == userId);
                }
                else
                {
                    parent = await _context.Parents.FirstOrDefaultAsync(x => x.Id == model.ParentId);
                }

                if (parent == null)
                {
                    await ParentsInViewBag();
                    return View(model);
                }
                Child child = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Parent = parent,
                    ParentId = parent.Id,
                };

                await _context.Children.AddAsync(child);
                parent.Children.Add(child);
                await _context.SaveChangesAsync();

                if (signInToCourse)
                {
                    return RedirectToAction("SignInToCourse", new { childId = child.Id });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            await ParentsInViewBag();
            return View(model);
        }

        [Authorize(Roles = "Parent, Admin")]
        public async Task<IActionResult> SignInToCourse(int childId)
        {
            var child = await _context.Children.FirstOrDefaultAsync(x => x.Id == childId);
            if (child == null)
            {
                return RedirectToAction("Add");
            }

            List<Course> courses = await _context.Courses
                .Where(x => x.CourseRequests.All(x => x.ChildId != child.Id))
                .Include(x => x.AgeGroup)
                .Include(x => x.CourseType)
                .Include(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .ToListAsync();

            RegisterChildForCourseViewModel model = new RegisterChildForCourseViewModel()
            {
                ChildId = childId,
                ChildName = $"{child.FirstName} {child.LastName}",
                Courses = courses
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Parent, Admin")]
        public async Task<IActionResult> SignInToCourse(
            RegisterChildForCourseViewModel model, int courseId)
        {
            if (ModelState.IsValid)
            {
                var course = _context.Courses.Include(x => x.CourseType).FirstOrDefault(x => x.Id == courseId);
                if (course == null)
                {
                    return View(model);
                }
                var child = _context.Children.FirstOrDefault(x => x.Id == model.ChildId);
                if (child == null)
                {
                    return View(model);
                }

                CourseRequest request = new()
                {
                    Course = course,
                    CourseId = courseId,
                    Child = child,
                    ChildId = model.ChildId,
                    Status = Data.Enums.RequestStatus.Pending
                };
                await _context.CourseRequests.AddAsync(request);
                course.CourseRequests.Add(request);
                child.CourseRequests.Add(request);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { message = $"Заявката за записване на детето {child.FirstName} {child.LastName} към курс {course.CourseType.Name}({course.StartDate.ToString("MMMM dd, yyyy", new CultureInfo("bg-BG"))} - {course.EndDate.ToString("MMMM dd, yyyy", new CultureInfo("bg-BG"))}) е изпратена успешно." });
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Parent")]
        public async Task<IActionResult> Edit(int childId)
        {
            var child = await _context.Children
                .Include(x => x.Parent)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == childId);

            if (child == null)
            {
                return RedirectToAction("Index");
            }

            if (User.IsInRole("Admin")
                || (User.IsInRole("Parent") && child.Parent.User.Id == GetUserId()))
            {
                AddChildViewModel model = new()
                {
                    Id = child.Id,
                    FirstName = child.FirstName,
                    LastName = child.LastName,
                    PhoneNumber = child.PhoneNumber,
                };
                if (User.IsInRole("Parent"))
                {
                    model.ParentId = child.Parent.Id;
                }
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Parent")]
        [HttpPost]
        public async Task<IActionResult> Edit(AddChildViewModel model)
        {
            var child = await _context.Children
                .Include(x => x.Parent)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (!ModelState.IsValid
                || child == null
                || (User.IsInRole("Parent") &&
                child.Parent.User.Id != GetUserId()))
            {
                return View(model);
            }
            child.FirstName = model.FirstName;
            child.LastName = model.LastName;
            child.PhoneNumber = model.PhoneNumber;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Authorize(Roles = "Admin, Employee, Parent")]
        public async Task<IActionResult> Info(int childId)
        {
            var child = await _context.Children
                .Include(x => x.Parent)
                .ThenInclude(x => x.User)
                .Include(x => x.CourseRequests)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.AgeGroup)
                .Include(x => x.CourseRequests)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.CourseType)
                .Include(x => x.CourseRequests)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.TeachersCourse)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == childId);

            if (child == null)
            {
                return RedirectToAction("Index");
            }

            return View(child);
        }

        [HttpGet]
        public IActionResult Delete(int childId)
        {
            var child = _context.Children
                .Include(x => x.CourseRequests)
                .Include(x => x.Parent)
                .FirstOrDefault(x => x.Id == childId);


            if (child == null)
            {
                return RedirectToAction("Index");
            }
            string message = $"Детето {child.FirstName} {child.LastName} беше изтрито успешно";
            var cr = child.CourseRequests.ToList();
            foreach (var request in cr)
            {
                child.CourseRequests.Remove(request);
                _context.CourseRequests.Remove(request);
            }
            var parent = _context.Parents.FirstOrDefault(x => x.Children.Contains(child));
            if (parent == null)
            {
                return RedirectToAction("Index"); ;
            }
            parent.Children.Remove(child);
            _context.Children.Remove(child);
            _context.SaveChanges();

            return RedirectToAction("Index", new { message = message });
        }

        public bool DeleteChild(int childId)
        {
            var child = _context.Children
                .Include(x => x.CourseRequests)
                .Include(x => x.Parent)
                .FirstOrDefault(x => x.Id == childId);
            if (child == null)
            {
                return false;
            }
            var cr = child.CourseRequests.ToList();
            foreach (var request in cr)
            {
                child.CourseRequests.Remove(request);
                _context.CourseRequests.Remove(request);
            }
            var parent = _context.Parents.FirstOrDefault(x => x.Children.Contains(child));
            if (parent == null)
            {
                return false;
            }
            parent.Children.Remove(child);
            _context.Children.Remove(child);
            _context.SaveChanges();

            return true;
        }

        public async Task ParentsInViewBag()
        {
            ViewBag.Parents = await _context.Parents.Include(x => x.User).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.User.FirstName + " " + x.User.LastName,
            }).ToListAsync();
        }
    }
}
