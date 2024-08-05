using Microsoft.AspNetCore.Mvc;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.Data;
using StudentEducationCenter.ViewModels.Additional;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;

namespace StudentEducationCenter.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
	public class CourseTypeController : BaseController
	{
		public CourseTypeController(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index(int? page,
			string? errorMessage = null,
			bool? sortByName = null,
            string? searchInput = null)
        {
			if (errorMessage != null)
			{
				ModelState.AddModelError(string.Empty, errorMessage);
			}
			List<CourseType> courseType = await _context.CourseTypes.OrderBy(x=>x.Name).ToListAsync();

            ViewBag.SortByName = null;
            ViewBag.SearchInput = null;

            if (searchInput != null && !string.IsNullOrEmpty(searchInput))
            {
                string temp = searchInput.TrimEnd().ToUpper();

                courseType = courseType.Where(x => x.Name.ToUpper().Contains(temp)).ToList();
                ViewBag.SearchInput = searchInput.TrimEnd();
            }
            else
            {
                // SORTING
                if (sortByName != null && sortByName == true)
                {
                    courseType = courseType.OrderBy(x => x.Name).ToList();
                    ViewBag.SortByName = true;
                }
                if (sortByName != null && sortByName == false)
                {
                    courseType = courseType.OrderByDescending(x => x.Name).ToList();
                    ViewBag.SortByName = false;
                }
            }

            courseType = Pagination(page, courseType, 6);

            return View(courseType);
		}

		public IActionResult Add()
		{
			return View(new CourseTypeViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Add(CourseTypeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			CourseType CourseType = new CourseType()
			{
				Name = model.CourseTypeName
			};
			await _context.CourseTypes.AddAsync(CourseType);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int id)
		{
			var courseType = await _context.CourseTypes.FirstOrDefaultAsync(c => c.Id == id);
			if (courseType != null)
			{
				if (_context.Courses.Any(x => x.CourseTypeId == courseType.Id))
				{
					return RedirectToAction(nameof(Index), new {errorMessage = $"Типът курс {courseType.Name} не може да бъде изтрит, защото съществува курс, регистриран с този тип." });
				}
				else
				{
					_context.CourseTypes.Remove(courseType);
					await _context.SaveChangesAsync();
				}
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
