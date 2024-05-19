using FishStoreApplication.Data;
using FishStoreApplication.Data.Migrations;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FishStoreApplication.Controllers
{
	public class CartController1 : Controller
	{
		private readonly ApplicationDbContext _context;
		private const string Cart = "ShopingCart";

		public CartController1(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Add(int id)
		{
			Fish? fishToAdd = _context.Fishes.Where(f => f.FishId == id).SingleOrDefault();
			if (fishToAdd == null)
			{
				TempData["Message"] = "Sorry that fish no longer exists";
				return RedirectToAction("Index", "Fishes");
			}
			CartFishViewModel cartFish = new()
			{
				FishId = fishToAdd.FishId,
				BreedName = fishToAdd.BreedName,
				Price = fishToAdd.Price
			};

			List<CartFishViewModel> cartFishes = GetExistingCartData();
			cartFishes.Add(cartFish);

			WriteShoppingCartCookie(cartFishes);

			TempData["Message"] = "Item added to cart";
			return RedirectToAction("Index", "Fishes");
		}
		public void WriteShoppingCartCookie(List<CartFishViewModel> cartFish)
		{
			string cookieData = JsonConvert.SerializeObject(cartFish);
			HttpContext.Response.Cookies.Append(Cart, cookieData, new CookieOptions()
			{
				Expires = DateTimeOffset.Now.AddYears(1)
			});
		}
		private List<CartFishViewModel> GetExistingCartData()
		{
			string? cookie = HttpContext.Request.Cookies[Cart];
			if (string.IsNullOrWhiteSpace(cookie))
			{
				return new List<CartFishViewModel>();
			}
			return JsonConvert.DeserializeObject<List<CartFishViewModel>>(cookie);
		}
		public IActionResult Summary()
		{
			List<CartFishViewModel> cartFishes = GetExistingCartData();
			return View(cartFishes);
		}
		public IActionResult Remove(int id)
		{
			List<CartFishViewModel> cartFishes = GetExistingCartData();
			CartFishViewModel? targetFish =
				cartFishes.Where(f => f.FishId == id).FirstOrDefault();
			cartFishes.Remove(targetFish);
			WriteShoppingCartCookie(cartFishes);
			return RedirectToAction(nameof(Summary));

		}
	}
}