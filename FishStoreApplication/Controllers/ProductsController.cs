using FishStoreApplication.Data;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FishStoreApplication.Controllers
{
    public class ProductsController : Controller
    {
		private readonly ApplicationDbContext _context;
		public ProductsController(ApplicationDbContext context)
		{
			_context = context;
		}
		[Authorize]
		public async Task<IActionResult> Index(int? id)
		{
			const int NumFishToDisplayPerPage = 6;
			const int PageOffSet = 1;
			int currPage = id ?? 1;

			int totalNumOfProducts = await _context.Fishes.CountAsync();
			double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumFishToDisplayPerPage);
			int lastPage = Convert.ToInt32(maxNumPages);

			
			List<Fish> fishes = await (from fish in _context.Fishes
									   select fish)
									   .Skip(NumFishToDisplayPerPage * (currPage - PageOffSet))
									   .Take(NumFishToDisplayPerPage)
									   .ToListAsync();
			FishCatalogViewModel catalogModel = new(fishes, lastPage, currPage);

			return View(catalogModel);
		}

	}
}