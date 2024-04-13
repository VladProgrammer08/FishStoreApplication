using FishStoreApplication.Data;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace FishStoreApplication.Controllers
{
    public class FishController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FishController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
