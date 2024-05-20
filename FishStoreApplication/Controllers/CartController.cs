using FishStoreApplication.Data;
using FishStoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FishStoreApplication.Controllers
{
	public class CartController : Controller
	{
        private readonly ApplicationDbContext _context;
        private const string Cart = "ShopingCart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Add(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = _context.Carts.Include(c => c.Items)
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

                var fishToAdd = _context.Fishes.SingleOrDefault(f => f.FishId == id);
                if (fishToAdd == null)
                {
                    TempData["Message"] = "Sorry that fish no longer exists";
                    return RedirectToAction("Index", "Products");
                }

                // Now we can safely use LINQ on cart.Items
                var cartItem = cart.Items.FirstOrDefault(ci => ci.FishId == fishToAdd.FishId);
                if (cartItem == null)
                {
                    cartItem = new CartItem { FishId = fishToAdd.FishId, Quantity = 1 };
                    cart.Items.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }

                _context.SaveChanges();

                TempData["Message"] = "Item added to cart";
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize]
        public IActionResult Summary()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItems = _context.CartItems
                                        .Include(ci => ci.Fish)
                                        .Where(ci => ci.Cart.UserId == userId)
                                        .ToList();

                var summaryViewModel = new CartSummaryViewModel
                {
                    Items = cartItems,
                    TotalPrice = cartItems.Sum(ci => ci.Fish.Price * ci.Quantity),
                    TaxRate = 0.10
                };

                return View(summaryViewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        public IActionResult Remove(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItem = _context.CartItems
                                       .Include(ci => ci.Cart)
                                       .FirstOrDefault(ci => ci.FishId == id && ci.Cart.UserId == userId);

                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Summary));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }



    }
}
		
		
		
	
