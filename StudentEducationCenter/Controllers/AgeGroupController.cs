using Microsoft.AspNetCore.Mvc;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.Data;
using StudentEducationCenter.ViewModels.Additional;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace StudentEducationCenter.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
	public class AgeGroupController : BaseController
	{
		public AgeGroupController(ApplicationDbContext context) : base(context)
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
			List<AgeGroup> ageGroups = await _context.AgeGroups.OrderBy(x=>x.Name).ToListAsync();

            ViewBag.SortByName = null;
            ViewBag.SearchInput = null;

            if (searchInput != null && !string.IsNullOrEmpty(searchInput))
            {
                string temp = searchInput.TrimEnd().ToUpper();

				ageGroups = ageGroups.Where(x => x.Name.ToUpper().Contains(temp)).ToList();
                ViewBag.SearchInput = searchInput.TrimEnd();
            }
            else
            {
                // SORTING
                if (sortByName != null && sortByName == true)
                {
                    ageGroups = ageGroups.OrderBy(x => x.Name).ToList();
                    ViewBag.SortByName = true;
                }
                if (sortByName != null && sortByName == false)
                {
                    ageGroups = ageGroups.OrderByDescending(x => x.Name).ToList();
                    ViewBag.SortByName = false;
                }
            }

            ageGroups = Pagination(page, ageGroups, 6);
			return View(ageGroups);
		}

		public IActionResult Add()
		{
			return View(new AgeGroupViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Add(AgeGroupViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			AgeGroup ageGroup = new AgeGroup()
			{
				Name = model.AgeGroupName
			};
			await _context.AgeGroups.AddAsync(ageGroup);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int id)
		{
			AgeGroup? ageGroup = _context.AgeGroups.FirstOrDefault(c => c.Id == id);
			if (ageGroup != null)
			{
				if (_context.Courses.Any(x => x.AgeGroupId == ageGroup.Id))
				{
					return RedirectToAction(nameof(Index), new { errorMessage = $"Възрастовата група {ageGroup.Name} не може да бъде изтрита, защото съществува курс, регистриран с нея." });
				}
				else
				{
					_context.AgeGroups.Remove(ageGroup);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			return RedirectToAction(nameof(Index));
		}
	}
}