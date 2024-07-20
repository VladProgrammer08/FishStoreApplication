using FishStoreApplication.Data;
using FishStoreApplication.Data.Migrations;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WishList = FishStoreApplication.Models.WishList;

namespace FishStoreApplication.Controllers
{
	public class WishListController : Controller
	{
		private readonly ApplicationDbContext _context;

		public WishListController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult AddToWishList(int id)
		{
			if (User.Identity.IsAuthenticated)
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var wishList = _context.WishLists.Include(w => w.Items)
												 .FirstOrDefault(w => w.UserId == userId);
				if (wishList == null)
				{
					wishList = new WishList
					{
						UserId = userId,
						Items = new List<WishListItem>()
					};
					_context.WishLists.Add(wishList); // Add the new wishlist to the context
				}
				else if (wishList.Items == null)
				{
					wishList.Items = new List<WishListItem>();
				}
				var productToAdd = _context.Fishes.SingleOrDefault(f => f.FishId == id);
				if (productToAdd == null)
				{
					TempData["Message"] = "Sorry, that fish no longer exists";
					return RedirectToAction("Index", "Products");
				}
				var wishListItem = wishList.Items.FirstOrDefault(wi => wi.FishId == productToAdd.FishId);
				if (wishListItem == null)
				{
					wishListItem = new WishListItem
					{
						FishId = productToAdd.FishId
					};
					wishList.Items.Add(wishListItem);
				}
				_context.SaveChanges();
				TempData["Message"] = "Item added to WishList";
				return RedirectToAction("Index", "Products");
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}


		public IActionResult WishListView()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishListItems = _context.WishListItems.Include(wi => wi.Fish)
                                            .Where(wi => wi.WishList.UserId == userId)
                                            .ToList();

                return View(wishListItems);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult RemoveFromWishList(int id)
		{
			if (User.Identity.IsAuthenticated)
			{
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var wishListItem = _context.WishListItems
											.Include(wi => wi.WishList)
											.FirstOrDefault(wi => wi.FishId == id && wi.WishList.UserId == userId);
				if (wishListItem != null)
				{
					_context.WishListItems.Remove(wishListItem);
					_context.SaveChanges();
				}
				return RedirectToAction(nameof(WishListView));
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}

    }
}
