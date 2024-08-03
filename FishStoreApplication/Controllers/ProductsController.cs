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
		public async Task<IActionResult> FishIndex(int? id, bool filterUnder10 = false, bool filter10to20 = false, bool filterSizeLessThan10 = false, bool filterSizeMoreThan10 = false)
		{
			const int NumFishToDisplayPerPage = 6;
			const int PageOffSet = 1;
			int currPage = id ?? 1;


			var fishQuery = _context.Fishes.AsQueryable();


			if (filterUnder10)
			{
				fishQuery = fishQuery.Where(f => f.Price <= 10);
			}
			if (filter10to20)
			{
				fishQuery = fishQuery.Where(f => f.Price > 10 && f.Price <= 20);
			}


			if (filterSizeLessThan10)
			{
				fishQuery = fishQuery.Where(f => f.FishSize < 10);
			}
			if (filterSizeMoreThan10)
			{
				fishQuery = fishQuery.Where(f => f.FishSize > 10);
			}

			int totalNumOfProducts = await fishQuery.CountAsync();
			double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumFishToDisplayPerPage);
			int lastPage = Convert.ToInt32(maxNumPages);

			List<Fish> fishes = await fishQuery
									   .Skip(NumFishToDisplayPerPage * (currPage - PageOffSet))
									   .Take(NumFishToDisplayPerPage)
									   .ToListAsync();
			FishCatalogViewModel catalogModel = new(fishes, lastPage, currPage);

			return View(catalogModel);
		}



		public async Task<IActionResult> FishDetails(int id)
		{
			Fish productDetails = await _context.Fishes.FindAsync(id);
			if (productDetails == null)
			{
				return NotFound();
			}
			return View(productDetails);
		}

	}
}