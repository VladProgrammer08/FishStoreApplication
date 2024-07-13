using FishStoreApplication.Data;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace FishStoreApplication.Controllers
{
    [Authorize(Roles = IdentityHelper.Admin)]
    public class FishController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public FishController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [Authorize(Roles = IdentityHelper.Admin)]
        public async Task<IActionResult> Index(int? id)
        {
            const int NumFishToDisplayPerPage = 6;
            const int PageOffSet = 1;
            int currPage = id ?? 1;

            int totalNumOfProducts = await _context.Fishes.CountAsync();
            double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumFishToDisplayPerPage);
            int lastPage = Convert.ToInt32(maxNumPages);

            // Get all fish from Db
            //List<Fish> fishes = await (from fish in _context.Fishes select fish).ToListAsync();
            List<Fish> fishes = await (from fish in _context.Fishes
                                       select fish)
                                       .Skip(NumFishToDisplayPerPage * (currPage - PageOffSet))
                                       .Take(NumFishToDisplayPerPage)
                                       .ToListAsync();
            FishCatalogViewModel catalogModel = new(fishes, lastPage, currPage);
            // Show them on the page
            return View(catalogModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FishViewModel f)
        {
            if(ModelState.IsValid)
            {
                if (f.FishUpload.MainImage != null)
                {
                    string mainImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(f.FishUpload.MainImage.FileName);

                    // Save file to file system
                    string mainImagePath = Path.Combine(_environment.WebRootPath, "images", mainImageFileName);
                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        await f.FishUpload.MainImage.CopyToAsync(stream);
                    }
                    // Generate unique file name
                    f.Fish.MainImageURL = "/images/" + mainImageFileName;
                }

                // Handle Secondary Images

                if (f.FishUpload.SecondaryImageOne != null)
                {
                    string secondaryImageOneFileName = Guid.NewGuid().ToString() + Path.GetExtension(f.FishUpload.SecondaryImageOne.FileName);
                    string secondaryImageOnePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageOneFileName);

                    using (var stream = new FileStream(secondaryImageOnePath, FileMode.Create))
                    {
                        await f.FishUpload.SecondaryImageOne.CopyToAsync(stream);
                    }

                    f.Fish.SecondaryImageOne = "/images/" + secondaryImageOneFileName;
                }
                if (f.FishUpload.SecondaryImageTwo != null)
                {
                    string secondaryImageTwoFileName = Guid.NewGuid().ToString() + Path.GetExtension(f.FishUpload.SecondaryImageTwo.FileName);
                    string secondaryImageTwoPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageTwoFileName);

                    using (var stream = new FileStream(secondaryImageTwoPath, FileMode.Create))
                    {
                        await f.FishUpload.SecondaryImageTwo.CopyToAsync(stream);
                    }

                    f.Fish.SecondaryImageTwo = "/images/" + secondaryImageTwoFileName;
                }
                if (f.FishUpload.SecondaryImageThree != null)
                {
                    string secondaryImageThreeFileName = Guid.NewGuid().ToString() + Path.GetExtension(f.FishUpload.SecondaryImageThree.FileName);
                    string secondaryImageThreePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageThreeFileName);

                    using (var stream = new FileStream(secondaryImageThreePath, FileMode.Create))
                    {
                        await f.FishUpload.SecondaryImageThree.CopyToAsync(stream);
                    }

                    f.Fish.SecondaryImageThree = "/images/" + secondaryImageThreeFileName;
                }
                if (f.FishUpload.SecondaryImageFour != null)
                {
                    string secondaryImageFourFileName = Guid.NewGuid().ToString() + Path.GetExtension(f.FishUpload.SecondaryImageFour.FileName);
                    string secondaryImageFourPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageFourFileName);

                    using (var stream = new FileStream(secondaryImageFourPath, FileMode.Create))
                    {
                        await f.FishUpload.SecondaryImageFour.CopyToAsync(stream);
                    }

                    f.Fish.SecondaryImageFour = "/images/" + secondaryImageFourFileName;
                }

                // Save fish to the database
                _context.Fishes.Add(f.Fish); // Prepares insert
                await _context.SaveChangesAsync(); // Executes pending insert

                ViewData["Message"] = $"{f.Fish.BreedName} was added successfully!";
                return View();
                
            }
            return View(f);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Fish? fishToEdit = await _context.Fishes.FindAsync(id);
            if (fishToEdit == null)
            {
                return NotFound();
            }

            var viewModel = new FishViewModel
            {
                Fish = fishToEdit,
                FishUpload = new CreateProductViewModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FishViewModel viewModel)
        {
            if (id != viewModel.Fish.FishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle Main Image
                    if (viewModel.FishUpload.MainImage != null)
                    {
                        string mainImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FishUpload.MainImage.FileName);
                        string mainImagePath = Path.Combine(_environment.WebRootPath, "images", mainImageFileName);

                        using (var stream = new FileStream(mainImagePath, FileMode.Create))
                        {
                            await viewModel.FishUpload.MainImage.CopyToAsync(stream);
                        }

                        viewModel.Fish.MainImageURL = "/images/" + mainImageFileName;
                    }

                    // Handle Secondary Images
                    if (viewModel.FishUpload.SecondaryImageOne != null)
                    {
                        string secondaryImageOneFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FishUpload.SecondaryImageOne.FileName);
                        string secondaryImageOnePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageOneFileName);

                        using (var stream = new FileStream(secondaryImageOnePath, FileMode.Create))
                        {
                            await viewModel.FishUpload.SecondaryImageOne.CopyToAsync(stream);
                        }

                        viewModel.Fish.SecondaryImageOne = "/images/" + secondaryImageOneFileName;
                    }

                    if (viewModel.FishUpload.SecondaryImageTwo != null)
                    {
                        string secondaryImageTwoFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FishUpload.SecondaryImageTwo.FileName);
                        string secondaryImageTwoPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageTwoFileName);

                        using (var stream = new FileStream(secondaryImageTwoPath, FileMode.Create))
                        {
                            await viewModel.FishUpload.SecondaryImageTwo.CopyToAsync(stream);
                        }

                        viewModel.Fish.SecondaryImageTwo = "/images/" + secondaryImageTwoFileName;
                    }

                    if (viewModel.FishUpload.SecondaryImageThree != null)
                    {
                        string secondaryImageThreeFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FishUpload.SecondaryImageThree.FileName);
                        string secondaryImageThreePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageThreeFileName);

                        using (var stream = new FileStream(secondaryImageThreePath, FileMode.Create))
                        {
                            await viewModel.FishUpload.SecondaryImageThree.CopyToAsync(stream);
                        }

                        viewModel.Fish.SecondaryImageThree = "/images/" + secondaryImageThreeFileName;
                    }

                    if (viewModel.FishUpload.SecondaryImageFour != null)
                    {
                        string secondaryImageFourFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.FishUpload.SecondaryImageFour.FileName);
                        string secondaryImageFourPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageFourFileName);

                        using (var stream = new FileStream(secondaryImageFourPath, FileMode.Create))
                        {
                            await viewModel.FishUpload.SecondaryImageFour.CopyToAsync(stream);
                        }

                        viewModel.Fish.SecondaryImageFour = "/images/" + secondaryImageFourFileName;
                    }

                    _context.Update(viewModel.Fish);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = $"{viewModel.Fish.BreedName} was updated successfully";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FishExists(viewModel.Fish.FishId))
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

        private bool FishExists(int id)
        {
            return _context.Fishes.Any(e => e.FishId == id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Fish? fishToDelete = await _context.Fishes.FindAsync(id);
            if(fishToDelete == null)
            {
                return NotFound();
            }
            return View(fishToDelete);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Fish? fishToDelete = await _context.Fishes.FindAsync(id);
            if(fishToDelete != null)
            {
                _context.Fishes.Remove(fishToDelete);
                await _context.SaveChangesAsync();
                TempData["Message"] = fishToDelete.BreedName + "was deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "This fish was already deleted";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            Fish fishDetails = await _context.Fishes.FindAsync(id);
            if(fishDetails == null)
            {
                return NotFound();
            }
            return View(fishDetails);
        }
    }
}
