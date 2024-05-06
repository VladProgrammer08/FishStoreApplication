using FishStoreApplication.Data;
using Microsoft.AspNetCore.Mvc;

namespace FishStoreApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
