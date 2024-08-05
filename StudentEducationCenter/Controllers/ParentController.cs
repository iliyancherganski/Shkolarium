using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using StudentEducationCenter.Data.Models;

namespace StudentEducationCenter.Controllers
{
    [Authorize(Roles = "Admin, Employee, Teacher")]
    public class ParentController : BaseController
    {
        public ParentController(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index(int? page, 
            bool? sortByName = null, 
            int cityId = 0, 
            string? searchInput = null)
        {
            List<Parent> parents = await _context.Parents
                .Include(x => x.Children)
                .Include(x => x.User)
                .ThenInclude(x => x.City)
                .ToListAsync();

            ViewBag.SortByName = null;
            ViewBag.CityId = cityId;
            ViewBag.SearchInput = null;

            await SortedCitiesInViewBag();

            if (searchInput != null && !string.IsNullOrEmpty(searchInput))
            {
                string temp = searchInput.TrimEnd().ToUpper();

                List<Parent> tempParents = new List<Parent>();

                foreach (var parent in parents)
                {
                    if ($"{parent.User.FirstName.ToUpper()} {parent.User.LastName.ToUpper()}".Contains(temp)
                        || (parent.User.PhoneNumber != null && parent.User.PhoneNumber.Contains(temp)))
                    {
                        tempParents.Add(parent);
                    }
                    else
                    {
                        foreach (var child in parent.Children)
                        {
                            if ($"{child.FirstName.ToUpper()} {child.LastName.ToUpper()}".Contains(temp))
                            {
                                tempParents.Add(parent);
                                break;
                            }
                        }
                    }
                }
                parents = tempParents;
                ViewBag.SearchInput = searchInput.TrimEnd();
            }
            else
            {
                // SORTING
                if (sortByName != null && sortByName == true)
                {
                    parents = parents.OrderBy(x => x.User.LastName).ThenBy(x => x.User.FirstName).ToList();
                    ViewBag.SortByName = true;
                }
                if (sortByName != null && sortByName == false)
                {
                    parents = parents.OrderBy(x => x.User.FirstName).ThenBy(x => x.User.LastName).ToList();
                    ViewBag.SortByName = false;
                }

                // FILTERING
                if (cityId != 0)
                {
                    parents = parents.Where(x => x.User.CityId == cityId).ToList();
                }
            }

            parents = Pagination(page, parents, 4);
            return View(parents);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int parentId)
        {
            var parent = await _context.Parents
                .Include(x => x.User)
                .Include(x => x.Children)
                .ThenInclude(x => x.CourseRequests)
                .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == parentId);
            if (parent != null)
            {
                var children = parent.Children.ToList();
                foreach (var child in children)
                {
                    if (child == null)
                    {
                        return View(nameof(Index));
                    }
                    var cr = child.CourseRequests.ToList();
                    foreach (var request in cr)
                    {
                        child.CourseRequests.Remove(request);
                        _context.CourseRequests.Remove(request);
                    }
                    parent.Children.Remove(child);
                    _context.Children.Remove(child);
                    _context.SaveChanges();
                }
                _context.Users.Remove(parent.User);
                _context.Parents.Remove(parent);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
