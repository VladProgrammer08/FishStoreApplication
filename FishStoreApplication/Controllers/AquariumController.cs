using FishStoreApplication.Data;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System.Linq.Expressions;

namespace FishStoreApplication.Controllers
{
	[Authorize(Roles = IdentityHelper.Admin)]
	public class AquariumController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _environment;
		public AquariumController(ApplicationDbContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		[Authorize(Roles = IdentityHelper.Admin)]
		public async Task<IActionResult> Index(int? id)
		{
			const int NumItemsToDisplayPerPage = 6;
			const int PageOffSet = 1;
			int currPage = id ?? 1;

			int totalNumOfProducts = await _context.Aquariums.CountAsync();
			double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumItemsToDisplayPerPage);
			int lastPage = Convert.ToInt32(maxNumPages);

			List<Aquarium> aquariums = await (from aquarium  in _context.Aquariums
											  select aquarium)
											  .Skip(NumItemsToDisplayPerPage * (currPage - PageOffSet))
											  .Take(NumItemsToDisplayPerPage)
											  .ToListAsync();
			AquariumCatalogViewModel catalogModel = new(aquariums, lastPage, currPage);
			return View(catalogModel);

		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(AquariumViewModel a)
		{
			if (ModelState.IsValid)
			{
				// Handle Main Image
				if (a.AquariumFileUpload.MainImage != null)
				{
					string mainImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(a.AquariumFileUpload.MainImage.FileName);

					// Save file to file system
					string mainImagePath = Path.Combine(_environment.WebRootPath, "images", mainImageFileName);
					using (var stream = new FileStream(mainImagePath, FileMode.Create))
					{
						await a.AquariumFileUpload.MainImage.CopyToAsync(stream);
					}
					// Generate unique file name
					a.Aquarium.MainImageURL = "/images/" + mainImageFileName;
				}
				// Handle Secondary Images

				if (a.AquariumFileUpload.SecondaryImageOne != null)
				{
					string secondaryImageOneFileName = Guid.NewGuid().ToString() + Path.GetExtension(a.AquariumFileUpload.SecondaryImageOne.FileName);
					string secondaryImageOnePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageOneFileName);

					using (var stream = new FileStream(secondaryImageOnePath, FileMode.Create))
					{
						await a.AquariumFileUpload.SecondaryImageOne.CopyToAsync(stream);
					}

					a.Aquarium.SecondaryImageOne = "/images/" + secondaryImageOneFileName;
				}
				if (a.AquariumFileUpload.SecondaryImageTwo != null)
				{
					string secondaryImageTwoFileName = Guid.NewGuid().ToString() + Path.GetExtension(a.AquariumFileUpload.SecondaryImageTwo.FileName);
					string secondaryImageTwoPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageTwoFileName);

					using (var stream = new FileStream(secondaryImageTwoPath, FileMode.Create))
					{
						await a.AquariumFileUpload.SecondaryImageTwo.CopyToAsync(stream);
					}

					a.Aquarium.SecondaryImageOne = "/images/" + secondaryImageTwoFileName;
				}
				if (a.AquariumFileUpload.SecondaryImageThree != null)
				{
					string secondaryImageThreeFileName = Guid.NewGuid().ToString() + Path.GetExtension(a.AquariumFileUpload.SecondaryImageThree.FileName);
					string secondaryImageThreePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageThreeFileName);

					using (var stream = new FileStream(secondaryImageThreePath, FileMode.Create))
					{
						await a.AquariumFileUpload.SecondaryImageThree.CopyToAsync(stream);
					}

					a.Aquarium.SecondaryImageOne = "/images/" + secondaryImageThreeFileName;
				}
				if (a.AquariumFileUpload.SecondaryImageFour != null)
				{
					string secondaryImageFourFileName = Guid.NewGuid().ToString() + Path.GetExtension(a.AquariumFileUpload.SecondaryImageFour.FileName);
					string secondaryImageFourPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageFourFileName);

					using (var stream = new FileStream(secondaryImageFourPath, FileMode.Create))
					{
						await a.AquariumFileUpload.SecondaryImageFour.CopyToAsync(stream);
					}

					a.Aquarium.SecondaryImageOne = "/images/" + secondaryImageFourFileName;
				}

				// Save aquarium to the database
				_context.Aquariums.Add(a.Aquarium);
				await _context.SaveChangesAsync();

				ViewData["MEssage"] = $"{a.Aquarium.AquariumBrand} was added successfully";
				return View();
			}
			return View(a);
		}

		// GET: Aquarium/Edit/5
		public async Task<ActionResult> Edit(int id)
		{
			Aquarium? aquariumToEdit = await _context.Aquariums.FindAsync(id);
			if (aquariumToEdit == null)
			{
				return NotFound();
			}

			var viewModel = new AquariumViewModel
			{
				Aquarium = aquariumToEdit,
				AquariumFileUpload = new AquariumFileUpload()
			};

			return View(viewModel);
		}

		// POST: Aquarium/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int id, AquariumViewModel viewModel)
		{
			if (id != viewModel.Aquarium.AquariumId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Handle Main Image
					if (viewModel.AquariumFileUpload.MainImage != null)
					{
						string mainImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.AquariumFileUpload.MainImage.FileName);

						// Save file to file system
						string mainImagePath = Path.Combine(_environment.WebRootPath, "images", mainImageFileName);
						using (var stream = new FileStream(mainImagePath, FileMode.Create))
						{
							await viewModel.AquariumFileUpload.MainImage.CopyToAsync(stream);
						}
						// Generate unique file name
						viewModel.Aquarium.MainImageURL = "/images/" + mainImageFileName;
					}
					// Handle Secondary Images

					if (viewModel.AquariumFileUpload.SecondaryImageOne != null)
					{
						string secondaryImageOneFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.AquariumFileUpload.SecondaryImageOne.FileName);
						string secondaryImageOnePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageOneFileName);

						using (var stream = new FileStream(secondaryImageOnePath, FileMode.Create))
						{
							await viewModel.AquariumFileUpload.SecondaryImageOne.CopyToAsync(stream);
						}

						viewModel.Aquarium.SecondaryImageOne = "/images/" + secondaryImageOneFileName;
					}
					if (viewModel.AquariumFileUpload.SecondaryImageTwo != null)
					{
						string secondaryImageTwoFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.AquariumFileUpload.SecondaryImageTwo.FileName);
						string secondaryImageTwoPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageTwoFileName);

						using (var stream = new FileStream(secondaryImageTwoPath, FileMode.Create))
						{
							await viewModel.AquariumFileUpload.SecondaryImageTwo.CopyToAsync(stream);
						}

						viewModel.Aquarium.SecondaryImageOne = "/images/" + secondaryImageTwoFileName;
					}
					if (viewModel.AquariumFileUpload.SecondaryImageThree != null)
					{
						string secondaryImageThreeFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.AquariumFileUpload.SecondaryImageThree.FileName);
						string secondaryImageThreePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageThreeFileName);

						using (var stream = new FileStream(secondaryImageThreePath, FileMode.Create))
						{
							await viewModel.AquariumFileUpload.SecondaryImageThree.CopyToAsync(stream);
						}

						viewModel.Aquarium.SecondaryImageOne = "/images/" + secondaryImageThreeFileName;
					}
					if (viewModel.AquariumFileUpload.SecondaryImageFour != null)
					{
						string secondaryImageFourFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.AquariumFileUpload.SecondaryImageFour.FileName);
						string secondaryImageFourPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageFourFileName);

						using (var stream = new FileStream(secondaryImageFourPath, FileMode.Create))
						{
							await viewModel.AquariumFileUpload.SecondaryImageFour.CopyToAsync(stream);
						}

						viewModel.Aquarium.SecondaryImageOne = "/images/" + secondaryImageFourFileName;
					}

					_context.Update(viewModel.Aquarium);
					await _context.SaveChangesAsync();

					TempData["Message"] = $"{viewModel.Aquarium.AquariumBrand} was updated successfully";
					return RedirectToAction("Index");
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AquariumExist(viewModel.Aquarium.AquariumId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}
			return View(viewModel);
		}

		private bool AquariumExist(int id)
		{
			return _context.Aquariums.Any(e => e.AquariumId == id);
		}

		public async Task<ActionResult> Delete(int id)
		{
			Aquarium? aquariumToDelete = await _context.Aquariums.FindAsync(id);
			if (aquariumToDelete == null)
			{
				return NotFound();
			}
			return View(aquariumToDelete);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			Aquarium? aquariumToDelete = await _context.Aquariums.FindAsync(id);
			if (aquariumToDelete != null)
			{
				_context.Aquariums.Remove(aquariumToDelete);
				await _context.SaveChangesAsync();
				TempData["Message"] = aquariumToDelete.AquariumBrand + "was deleted successfully";
				return RedirectToAction("Index");
			}
			TempData["Message"] = "This aquarium was already deleted";
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> Details(int id)
		{
			Aquarium aquariumDetails = await _context.Aquariums.FindAsync(id);
			if (aquariumDetails == null)
			{
				return NotFound();
			}
			return View(aquariumDetails);
		}
	}
}
