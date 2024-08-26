using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishStoreApplication.Controllers
{
    public class ProductCatalogController : Controller
    {
        // Index page of a Product Catalog
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}