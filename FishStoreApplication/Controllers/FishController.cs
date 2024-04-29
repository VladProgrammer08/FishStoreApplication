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
        public FishController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = IdentityHelper.Admin)]
        public async Task<IActionResult> Index()
        {
            // Get all fish from Db
            List<Fish> fishes = await (from fish in _context.Fishes select fish).ToListAsync();
            // Show them on the page
            return View(fishes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Fish f)
        {
            if(ModelState.IsValid)
            {
                _context.Fishes.Add(f); // Prepares insert
                await _context.SaveChangesAsync(); // Executes pending insert

                ViewData["Message"] = $"{f.BreedName} was added successfully!";
                return View();
            }
            return View(f);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Fish? fishToEdit = await _context.Fishes.FindAsync(id);
            if(fishToEdit == null)
            {
                return NotFound();
            }
            return View(fishToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Fish fishModel)
        {
            if(ModelState.IsValid)
            {
                _context.Fishes.Update(fishModel);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{fishModel.BreedName} was updated successfully";
                return RedirectToAction("Index");
            }
            return View(fishModel);
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
