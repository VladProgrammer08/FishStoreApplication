using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishStoreApplication.Controllers
{
    public class ProductCatalogController : Controller
    {
        [Authorize(Roles = IdentityHelper.Admin)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
