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
                if (productToAdd == null)
                {
                    TempData["Message"] = "Sorry, that product no longer exists";
                    return RedirectToAction("Index", "Products");
                }

                string redirectAction = "Index";
                string redirectController = "Products";

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

                _context.SaveChanges();

                TempData["Message"] = "Item added to cart";
                return RedirectToAction(redirectAction, redirectController);
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
                                        .Include(ci => ci.Product)
                                        .Where(ci => ci.Cart.UserId == userId)
                                        .ToList();

                var summaryViewModel = new CartSummaryViewModel
                {
                    Items = cartItems,
                    TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
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
                                       .FirstOrDefault(ci => ci.ProductId == id && ci.Cart.UserId == userId);

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


        public IActionResult DecreaseQuantity(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItem = _context.CartItems
                                       .Include(ci => ci.Cart)
                                       .FirstOrDefault(ci => ci.ProductId == id && ci.Cart.UserId == userId);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                    }
                    else
                    {
                        _context.CartItems.Remove(cartItem);
                    }

                    _context.SaveChanges();
                }             

                return RedirectToAction(nameof(Summary));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }



        public async Task<IActionResult> CheckoutAsync()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _context.Carts.Include(c => c.Items)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart != null && cart.Items.Any())
            {
                _context.CartItems.RemoveRange(cart.Items);
                _context.SaveChanges();
                TempData["Message"] = "Thank you for shopping with us!";
            }
            return RedirectToAction("Index", "ProductCatalog");
        }

        public IActionResult ShoppingAction()
        {

            return RedirectToAction("Index", "ProductCatalog");
        }

    }
}
		
		
		
	
