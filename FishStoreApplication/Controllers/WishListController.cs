using FishStoreApplication.Data;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Cart = FishStoreApplication.Models.Cart;
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

        // Add item to a WishList page
        public IActionResult AddToWishList(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishList = _context.WishLists.Include(w => w.Items)
                                                 .ThenInclude(wi => wi.Product)
                                                 .FirstOrDefault(w => w.UserId == userId);
                if (wishList == null)
                {
                    wishList = new WishList
                    {
                        UserId = userId,
                        Items = new List<WishListItem>()
                    };
                    _context.WishLists.Add(wishList);
                }
                else if (wishList.Items == null)
                {
                    wishList.Items = new List<WishListItem>();
                }

                var productToAdd = _context.Products.SingleOrDefault(p => p.Id == id);
                if (productToAdd == null)
                {
                    TempData["Message"] = "Sorry, that product no longer exists";
                    return RedirectToAction("Index", "ProductCatalog");
                }

                string redirectAction = "Index";
                string redirectController = "ProductCatalog";

                if (productToAdd is Fish)
                {
                    redirectAction = "FishIndex";
                }
                else if (productToAdd is Aquarium)
                {
                    redirectAction = "AquariumIndex";
                }
                if (productToAdd is Decoration)
                {
                    redirectAction = "DecorationIndex";
                }

                var wishListItem = wishList.Items.FirstOrDefault(wi => wi.ProductId == productToAdd.Id);
                if (wishListItem == null)
                {
                    wishListItem = new WishListItem
                    {
                        ProductId = productToAdd.Id,
                        Product = productToAdd
                    };
                    wishList.Items.Add(wishListItem);
                }

                _context.SaveChanges();
                TempData["Message"] = "Item added to WishList";
                return RedirectToAction(redirectAction, redirectController);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // Wishlist view
        [Authorize]
        public IActionResult WishListView()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string userName = User.Identity.Name;

                var wishListItems = _context.WishListItems.Include(wi => wi.Product)
                                            .Where(wi => wi.WishList.UserId == userId)
                                            .ToList();

                ViewData["Title"] = $"{userName}'s Wishlist";

                return View(wishListItems);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // Remove item from a WishList
        public IActionResult RemoveFromWishList(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishListItem = _context.WishListItems
                                           .Include(wi => wi.WishList)
                                           .FirstOrDefault(wi => wi.ProductId == id && wi.WishList.UserId == userId);
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

        // Toggle the WishList
        [HttpPost]
        public IActionResult ToggleWishList(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishList = _context.WishLists.Include(w => w.Items)
                                                 .ThenInclude(wi => wi.Product)
                                                 .FirstOrDefault(w => w.UserId == userId);
                if (wishList == null)
                {
                    wishList = new WishList
                    {
                        UserId = userId,
                        Items = new List<WishListItem>()
                    };
                    _context.WishLists.Add(wishList);
                }

                var wishListItem = wishList.Items.FirstOrDefault(wi => wi.ProductId == id);
                if (wishListItem == null)
                {
                    var productToAdd = _context.Products.SingleOrDefault(p => p.Id == id);
                    string redirectAction = "Index";
                    string redirectController = "ProductCatalog";

                    if (productToAdd is Fish)
                    {
                        redirectAction = "FishIndex";
                    }
                    else if (productToAdd is Aquarium)
                    {
                        redirectAction = "AquariumIndex";
                    }
                    if (productToAdd is Decoration)
                    {
                        redirectAction = "DecorationIndex";
                    }

                    if (productToAdd == null)
                    {
                        TempData["Message"] = "Sorry, that product no longer exists";
                        return RedirectToAction(redirectAction, redirectController);
                    }

                    wishListItem = new WishListItem
                    {
                        ProductId = productToAdd.Id,
                        Product = productToAdd
                    };
                    wishList.Items.Add(wishListItem);
                    TempData["Message"] = "Item added to WishList";
                }
                else
                {
                    _context.WishListItems.Remove(wishListItem);
                    TempData["Message"] = "Item removed from WishList";
                }

                _context.SaveChanges();
                return RedirectToAction("WishListView", "WishList");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        
        // Check for the same item in the WishList
        [HttpGet]
        public JsonResult IsInWishList(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isInWishList = _context.WishListItems
                                           .Any(wi => wi.ProductId == id && wi.WishList.UserId == userId);
                return Json(new { isInWishList });
            }
            return Json(new { isInWishList = false });
        }

        // Add an item from WishList to Cart page - remove from WishList
        public IActionResult Add(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = _context.Carts.Include(c => c.Items)
                                         .ThenInclude(i => i.Product)
                                         .FirstOrDefault(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                    _context.Carts.Add(cart);
                }
                else if (cart.Items == null)
                {
                    cart.Items = new List<CartItem>();
                }

                var productToAdd = _context.Products.SingleOrDefault(p => p.Id == id);
                string redirectAction = "Index";
                string redirectController = "ProductCatalog";

                if (productToAdd is Fish)
                {
                    redirectAction = "FishIndex";
                }
                else if (productToAdd is Aquarium)
                {
                    redirectAction = "AquariumIndex";
                }
                if (productToAdd is Decoration)
                {
                    redirectAction = "DecorationIndex";
                }

                if (productToAdd == null)
                {
                    TempData["Message"] = "Sorry, that product no longer exists";
                    return RedirectToAction(redirectAction, redirectController);
                }

                var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productToAdd.Id);
                if (cartItem == null)
                {
                    cartItem = new CartItem { ProductId = productToAdd.Id, Product = productToAdd, Quantity = 1 };
                    cart.Items.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }

                var wishList = _context.WishLists.Include(w => w.Items)
                                                 .ThenInclude(wi => wi.Product)
                                                 .FirstOrDefault(w => w.UserId == userId);
                if (wishList != null)
                {
                    var wishListItem = wishList.Items.FirstOrDefault(wi => wi.ProductId == id);
                    if (wishListItem != null)
                    {
                        wishList.Items.Remove(wishListItem);
                    }
                }

                _context.SaveChanges();

                TempData["Message"] = "Item added to cart and removed from wishlist";
                return RedirectToAction("WishListView", "WishList");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}
