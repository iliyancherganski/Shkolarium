using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEducationCenter.Data;
using System.Security.Claims;

namespace StudentEducationCenter.Controllers
{
    public class BaseController : Controller
    {
        public ApplicationDbContext _context;
        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }
        public string? GetUserId()
        {
            string? id = string.Empty;

            if (User != null)
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return id;
        }

        public List<T> Pagination<T>(int? page, List<T> list, int elementsOnPage)
        {
            if (page == null || elementsOnPage < page * elementsOnPage - list.Count())
            {
                page = 1;
            }
            int pagecount = list.Count() / elementsOnPage;
            if (list.Count() % elementsOnPage > 0)
            {
                pagecount++;
            }

            var tempList = new List<T>();
            int itemsOnPage;
            if (page < pagecount)
            {
                itemsOnPage = elementsOnPage;
            }
            else
            {
                itemsOnPage = list.Count() - (elementsOnPage * ((int)page-1));
            }

            for (int i = ((int)page - 1) * elementsOnPage; i < ((int)page - 1) * elementsOnPage + itemsOnPage; i++)
            {
                tempList.Add(list[i]);
            }

            ViewBag.Page = (int)page;
            ViewBag.PageCount = pagecount;

            return tempList;
        }

        public async Task SortedCitiesInViewBag()
        {
            ViewBag.Cities = await _context.Cities.OrderBy(x => x.Name).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToListAsync();
        }
    }
}
