﻿using FishStoreApplication.Data;
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
		// Fish Index page
		[Authorize]
		public async Task<IActionResult> FishIndex(int? id, bool filterUnder10 = false, bool filter10to20 = false, bool filterLengthLessThan10 = false, bool filterLengthMoreThan10 = false)
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


			if (filterLengthLessThan10)
			{
				fishQuery = fishQuery.Where(f => f.Length < 10);
			}
			if (filterLengthMoreThan10)
			{
				fishQuery = fishQuery.Where(f => f.Length > 10);
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


        // Fish Details page
        public async Task<IActionResult> FishDetails(int id)
		{
			Fish productDetails = await _context.Fishes.FindAsync(id);
			if (productDetails == null)
			{
				return NotFound();
			}
			return View(productDetails);
		}


        // Aquarium Index page
        [Authorize]
		public async Task<IActionResult> AquariumIndex(int? id, bool filterUnder50 = false, bool filter50to100 = false, bool filterVolumeLessThan20 = false, bool filterVolumeMoreThan20 = false)
		{
			const int NumAquariumToDisplayPerPage = 6;
			const int PageOffSet = 1;
			int currPage = id ?? 1;

			var aquariumQuerry = _context.Aquariums.AsQueryable();

			if (filterUnder50)
			{
				aquariumQuerry = aquariumQuerry.Where(a => a.Price <= 50);
			}
			if (filter50to100)
			{
				aquariumQuerry = aquariumQuerry.Where(a => a.Price >= 50 && a.Price <= 100);
			}

			if (filterVolumeLessThan20)
			{
				aquariumQuerry = aquariumQuerry.Where(a => a.AquariumVolume < 20);
			}
			if (filterVolumeMoreThan20)
			{
				aquariumQuerry = aquariumQuerry.Where(a => a.AquariumVolume > 20);
			}

			int totalNumOfProducts = await aquariumQuerry.CountAsync();
			double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumAquariumToDisplayPerPage);
			int lastPage = Convert.ToInt32(maxNumPages);

			List<Aquarium> aquariums = await aquariumQuerry
												.Skip(NumAquariumToDisplayPerPage * (currPage - PageOffSet))
												.Take(NumAquariumToDisplayPerPage)
												.ToListAsync();
			AquariumCatalogViewModel catalogModel = new(aquariums, lastPage, currPage);

			return View(catalogModel);

		}

        // Aquarium Details page
        public async Task<IActionResult> AquariumDetails(int id)
        {
            Aquarium productDetails = await _context.Aquariums.FindAsync(id);
            if (productDetails == null)
            {
                return NotFound();
            }
            return View(productDetails);
        }

        // Decoration Index page
        [Authorize]
        public async Task<IActionResult> DecorationIndex(int? id, bool filterUnder10 = false, bool filter10to20 = false, bool filterLengthLessThan10 = false, bool filterLengthMoreThan10 = false)
        {
            const int NumDecorationToDisplayPerPage = 6;
            const int PageOffSet = 1;
            int currPage = id ?? 1;


            var decorationQuery = _context.Decorations.AsQueryable();


            if (filterUnder10)
            {
                decorationQuery = decorationQuery.Where(f => f.Price <= 10);
            }
            if (filter10to20)
            {
                decorationQuery = decorationQuery.Where(f => f.Price > 10 && f.Price <= 20);
            }


            if (filterLengthLessThan10)
            {
                decorationQuery = decorationQuery.Where(f => f.Length < 10);
            }
            if (filterLengthMoreThan10)
            {
                decorationQuery = decorationQuery.Where(f => f.Length > 10);
            }

            int totalNumOfProducts = await decorationQuery.CountAsync();
            double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumDecorationToDisplayPerPage);
            int lastPage = Convert.ToInt32(maxNumPages);

            List<Decoration> decorations = await decorationQuery
                                       .Skip(NumDecorationToDisplayPerPage * (currPage - PageOffSet))
                                       .Take(NumDecorationToDisplayPerPage)
                                       .ToListAsync();
            DecorationCatalogViewModel catalogModel = new(decorations, lastPage, currPage);

            return View(catalogModel);
        }

        // Decoration Detail page
        public async Task<IActionResult> DecorationDetails(int id)
		{
			Decoration productDetails = await _context.Decorations.FindAsync(id);
			if (productDetails == null)
			{
				return NotFound();
			}
			return View(productDetails);
		}

	}
}