using FishStoreApplication.Data;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FishStoreApplication.Controllers
{
    [Authorize(Roles = IdentityHelper.Admin)]
    public class DecorationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public DecorationController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [Authorize(Roles = IdentityHelper.Admin)]

        public async Task<IActionResult> Index(int? id)
        {
            const int NumDecorationToDisplayPerPage = 6;
            const int PageOffSet = 1;
            int currPage = id ?? 1;

            int totalNumOfProducts = await _context.Decorations.CountAsync();
            double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumDecorationToDisplayPerPage);
            int lastPage = Convert.ToInt32(maxNumPages);

            List<Decoration> decorations = await (from decoration in _context.Decorations
                                                  select decoration)
                                                  .Skip(NumDecorationToDisplayPerPage * (currPage - PageOffSet))
                                                  .Take(NumDecorationToDisplayPerPage)
                                                  .ToListAsync();
            DecorationCatalogViewModel catalogModel = new(decorations, lastPage, currPage);

            return View(catalogModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DecorationViewModel d)
        {
            if (ModelState.IsValid)
            {
                if (d.DecorationFileUpload.MainImage != null)
                {
                    string mainImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(d.DecorationFileUpload.MainImage.FileName);

                    // Save file to file system
                    string mainImagePath = Path.Combine(_environment.WebRootPath, "images", mainImageFileName);
                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        await d.DecorationFileUpload.MainImage.CopyToAsync(stream);
                    }
                    // Generate unique file name
                    d.Decoration.MainImageURL = "/images/" + mainImageFileName;
                }

                // Handle Secondary Images

                if (d.DecorationFileUpload.SecondaryImageOne != null)
                {
                    string secondaryImageOneFileName = Guid.NewGuid().ToString() + Path.GetExtension(d.DecorationFileUpload.SecondaryImageOne.FileName);
                    string secondaryImageOnePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageOneFileName);

                    using (var stream = new FileStream(secondaryImageOnePath, FileMode.Create))
                    {
                        await d.DecorationFileUpload.SecondaryImageOne.CopyToAsync(stream);
                    }

                    d.Decoration.SecondaryImageOne = "/images/" + secondaryImageOneFileName;
                }
                if (d.DecorationFileUpload.SecondaryImageTwo != null)
                {
                    string secondaryImageTwoFileName = Guid.NewGuid().ToString() + Path.GetExtension(d.DecorationFileUpload.SecondaryImageTwo.FileName);
                    string secondaryImageTwoPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageTwoFileName);

                    using (var stream = new FileStream(secondaryImageTwoPath, FileMode.Create))
                    {
                        await d.DecorationFileUpload.SecondaryImageTwo.CopyToAsync(stream);
                    }

                    d.Decoration.SecondaryImageTwo = "/images/" + secondaryImageTwoFileName;
                }
                if (d.DecorationFileUpload.SecondaryImageThree != null)
                {
                    string secondaryImageThreeFileName = Guid.NewGuid().ToString() + Path.GetExtension(d.DecorationFileUpload.SecondaryImageThree.FileName);
                    string secondaryImageThreePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageThreeFileName);

                    using (var stream = new FileStream(secondaryImageThreePath, FileMode.Create))
                    {
                        await d.DecorationFileUpload.SecondaryImageThree.CopyToAsync(stream);
                    }

                    d.Decoration.SecondaryImageThree = "/images/" + secondaryImageThreeFileName;
                }
                if (d.DecorationFileUpload.SecondaryImageFour != null)
                {
                    string secondaryImageFourFileName = Guid.NewGuid().ToString() + Path.GetExtension(d.DecorationFileUpload.SecondaryImageFour.FileName);
                    string secondaryImageFourPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageFourFileName);

                    using (var stream = new FileStream(secondaryImageFourPath, FileMode.Create))
                    {
                        await d.DecorationFileUpload.SecondaryImageFour.CopyToAsync(stream);
                    }

                    d.Decoration.SecondaryImageFour = "/images/" + secondaryImageFourFileName;
                }

                // Save decoration to the database
                _context.Decorations.Add(d.Decoration); // Prepares insert
                await _context.SaveChangesAsync(); // Executes pending insert

                ViewData["Message"] = $"{d.Decoration.Name} was added successfully!";
                return View();

            }
            return View(d);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Decoration? decorationToEdit = await _context.Decorations.FindAsync(id);
            if (decorationToEdit == null)
            {
                return NotFound();
            }

            var viewModel = new DecorationViewModel
            {
                Decoration = decorationToEdit,
                DecorationFileUpload = new DecorationFileUpload()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DecorationViewModel viewModel)
        {
            if (id != viewModel.Decoration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle Main Image
                    if (viewModel.DecorationFileUpload.MainImage != null)
                    {
                        string mainImageFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.DecorationFileUpload.MainImage.FileName);
                        string mainImagePath = Path.Combine(_environment.WebRootPath, "images", mainImageFileName);

                        using (var stream = new FileStream(mainImagePath, FileMode.Create))
                        {
                            await viewModel.DecorationFileUpload.MainImage.CopyToAsync(stream);
                        }

                        viewModel.Decoration.MainImageURL = "/images/" + mainImageFileName;
                    }

                    // Handle Secondary Images
                    if (viewModel.DecorationFileUpload.SecondaryImageOne != null)
                    {
                        string secondaryImageOneFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.DecorationFileUpload.SecondaryImageOne.FileName);
                        string secondaryImageOnePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageOneFileName);

                        using (var stream = new FileStream(secondaryImageOnePath, FileMode.Create))
                        {
                            await viewModel.DecorationFileUpload.SecondaryImageOne.CopyToAsync(stream);
                        }

                        viewModel.Decoration.SecondaryImageOne = "/images/" + secondaryImageOneFileName;
                    }

                    if (viewModel.DecorationFileUpload.SecondaryImageTwo != null)
                    {
                        string secondaryImageTwoFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.DecorationFileUpload.SecondaryImageTwo.FileName);
                        string secondaryImageTwoPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageTwoFileName);

                        using (var stream = new FileStream(secondaryImageTwoPath, FileMode.Create))
                        {
                            await viewModel.DecorationFileUpload.SecondaryImageTwo.CopyToAsync(stream);
                        }

                        viewModel.Decoration.SecondaryImageTwo = "/images/" + secondaryImageTwoFileName;
                    }

                    if (viewModel.DecorationFileUpload.SecondaryImageThree != null)
                    {
                        string secondaryImageThreeFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.DecorationFileUpload.SecondaryImageThree.FileName);
                        string secondaryImageThreePath = Path.Combine(_environment.WebRootPath, "images", secondaryImageThreeFileName);

                        using (var stream = new FileStream(secondaryImageThreePath, FileMode.Create))
                        {
                            await viewModel.DecorationFileUpload.SecondaryImageThree.CopyToAsync(stream);
                        }

                        viewModel.Decoration.SecondaryImageThree = "/images/" + secondaryImageThreeFileName;
                    }

                    if (viewModel.DecorationFileUpload.SecondaryImageFour != null)
                    {
                        string secondaryImageFourFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.DecorationFileUpload.SecondaryImageFour.FileName);
                        string secondaryImageFourPath = Path.Combine(_environment.WebRootPath, "images", secondaryImageFourFileName);

                        using (var stream = new FileStream(secondaryImageFourPath, FileMode.Create))
                        {
                            await viewModel.DecorationFileUpload.SecondaryImageFour.CopyToAsync(stream);
                        }

                        viewModel.Decoration.SecondaryImageFour = "/images/" + secondaryImageFourFileName;
                    }

                    _context.Update(viewModel.Decoration);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = $"{viewModel.Decoration.Name} was updated successfully";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DecorationExists(viewModel.Decoration.Id))
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

        private bool DecorationExists(int id)
        {
            return _context.Decorations.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Decoration? decorationToDelete = await _context.Decorations.FindAsync(id);
            if (decorationToDelete == null)
            {
                return NotFound();
            }
            return View(decorationToDelete);
        }
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Decoration? decorationToDelete = await _context.Decorations.FindAsync(id);
            if (decorationToDelete != null)
            {
                _context.Decorations.Remove(decorationToDelete);
                await _context.SaveChangesAsync();
                TempData["Message"] = decorationToDelete.Name + "was deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "This decoration was already deleted";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            Decoration decorationDetails = await _context.Decorations.FindAsync(id);
            if (decorationDetails == null)
            {
                return NotFound();
            }
            return View(decorationDetails);
        }
        public IActionResult BackToManage()
        {
            return RedirectToAction("Index", "ManageProduct");
        }
    }
}
