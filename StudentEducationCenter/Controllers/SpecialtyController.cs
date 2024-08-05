using Microsoft.AspNetCore.Mvc;
using StudentEducationCenter.Data.Models;
using StudentEducationCenter.Data;
using StudentEducationCenter.ViewModels.Additional;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;

namespace StudentEducationCenter.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class SpecialtyController : BaseController
    {
        public SpecialtyController(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index(int? page,
            bool? sortByName = null,
            string? searchInput = null)
        {
            List<Specialty> specialties = await _context.Specialties.OrderBy(x => x.Name).ToListAsync();

            ViewBag.SortByName = null;
            ViewBag.SearchInput = null;

            if (searchInput != null && !string.IsNullOrEmpty(searchInput))
            {
                string temp = searchInput.TrimEnd().ToUpper();

                specialties = specialties.Where(x => x.Name.ToUpper().Contains(temp)).ToList();
                ViewBag.SearchInput = searchInput.TrimEnd();
            }
            else
            {
                // SORTING
                if (sortByName != null && sortByName == true)
                {
                    specialties = specialties.OrderBy(x => x.Name).ToList();
                    ViewBag.SortByName = true;
                }
                if (sortByName != null && sortByName == false)
                {
                    specialties = specialties.OrderByDescending(x => x.Name).ToList();
                    ViewBag.SortByName = false;
                }
            }

            specialties = Pagination(page, specialties, 6);
            return View(specialties);
        }

        public IActionResult Add()
        {
            return View(new SpecialtyViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpecialtyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Specialty position = new Specialty()
            {
                Name = model.SpecialtyName
            };
            await _context.Specialties.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var specialty = await _context.Specialties.FirstOrDefaultAsync(c => c.Id == id);
            if (specialty != null)
            {
                _context.Specialties.Remove(specialty);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
