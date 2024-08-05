using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.ViewModels.Additional;

namespace StudentEducationCenter.Controllers
{
	[Authorize(Roles = "Admin, Employee")]
	public class CityController : BaseController
	{
		public CityController(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index(int? page, 
			string? errorMessage = null,
			bool ? sortByName = null,
            string? searchInput = null)
        {
			if (errorMessage != null)
			{
				ModelState.AddModelError(string.Empty, errorMessage);
			}

            List<City> cities = await _context.Cities.OrderBy(x=>x.Name).ToListAsync();

            ViewBag.SortByName = null;
            ViewBag.SearchInput = null;

            if (searchInput != null && !string.IsNullOrEmpty(searchInput))
            {
                string temp = searchInput.TrimEnd().ToUpper();

                cities = cities.Where(x => x.Name.ToUpper().Contains(temp)).ToList();
                ViewBag.SearchInput = searchInput.TrimEnd();
            }
            else
            {
                // SORTING
                if (sortByName != null && sortByName == true)
                {
                    cities = cities.OrderBy(x => x.Name).ToList();
                    ViewBag.SortByName = true;
                }
                if (sortByName != null && sortByName == false)
                {
                    cities = cities.OrderByDescending(x => x.Name).ToList();
                    ViewBag.SortByName = false;
                }
            }

			cities = Pagination(page, cities, 6);
			return View(cities);
		}

		public IActionResult Add()
		{
			return View(new CityViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Add(CityViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			City city = new City()
			{
				Name = model.CityName
			};
			await _context.Cities.AddAsync(city);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int id)
		{
			var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
			if (city != null)
			{
				if (_context.Users.Any(x => x.CityId == city.Id))
				{
					return RedirectToAction(nameof(Index), new { errorMessage = $"Град {city.Name} не може да бъде изтрит, защото има потребители, регистрирани с този град." });
				}
				else
				{
					_context.Cities.Remove(city);
					await _context.SaveChangesAsync();
				}
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
