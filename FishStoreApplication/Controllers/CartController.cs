using FishStoreApplication.Data;
using FishStoreApplication.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Drawing;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FishStoreApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string Cart = "ShopingCart";
        private readonly IConfiguration _configuration;



        public CartController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Add item to a Cart
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



        // A summary view of a Cart
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


        // Remove item from a Cart
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

        // Decrease quantity of an item in the Cart
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

        // Checkout a Cart - Empty a Cart - Send an email receipt
        [HttpPost]
        public async Task<IActionResult> CheckoutAsync()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _context.Carts.Include(c => c.Items)
                                     .FirstOrDefault(c => c.UserId == userId);
            string userName = User.Identity.Name;
            if (cart != null && cart.Items.Any())
            {
                // Get the user's email address
                var userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmailClaim))
                {
                    TempData["Message"] = "Email address not found.";
                    return RedirectToAction("Summary", "Cart");
                }

                // Render the email template
                var cartItems = _context.CartItems
                                        .Include(ci => ci.Product)
                                        .Where(ci => ci.Cart.UserId == userId)
                                        .ToList();
                var model = new CartSummaryViewModel
                {
                    Items = cartItems,
                    TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                    TaxRate = 0.10
                };
                var emailBody = new StringBuilder();
                emailBody.AppendLine("<div style=\"width: 40%; margin: 0 auto; border: 1px solid #ccc; padding: 30px; border-radius: 5px;\"\">");
                emailBody.AppendLine("<h2 style=\"text-align: center; color: #0074D9;\">Aqua World</h2>");
                emailBody.AppendLine("<h1 style=\"text-align: center; font-weight: bold; color: black;\">Order Confirmation</h1>");
                emailBody.AppendLine($"<p style=\"text-align: center; font-size: 15px; font-weight: bold; color: black;\">Hello,{userName}. We received your order and we'll let you know when we ship it out</p>");
                emailBody.AppendLine("<hr style=\"border: 1px solid #ccc;\" />");;
                emailBody.AppendLine($"<p style=\"font-size: 15px; font-weight: bold; color: black;\">Your Order (#XXXXXXX):</p>");
                emailBody.AppendLine("<hr style=\"border: 1px solid #ccc;\" />");
                emailBody.AppendLine("<table style=\"width: 100%; border-collapse: collapse;\">");

                bool isEvenRow = false;

                foreach (var item in model.Items)
                {
                    string rowColor = isEvenRow ? "#f2f2f2" : "#ffffff";
                    isEvenRow = !isEvenRow;

                    emailBody.AppendLine("<tr>");
                    emailBody.AppendLine($"<td style=\"padding: 8px; color: black; background-color: {rowColor};\">{item.Quantity} X {item.Product.Name}</td>");
                    emailBody.AppendLine($"<td style=\"padding: 8px; color: black; background-color: {rowColor};\">{item.Product.Price:C}</td>");
                    emailBody.AppendLine("</tr>");
                }
                emailBody.AppendLine("</table>");
                emailBody.AppendLine("<hr style=\"border: 1px solid #ccc;\" />");

                // 2nd table
                emailBody.AppendLine("<table style=\"width: 100%; border-collapse: collapse;\">");
                emailBody.AppendLine("<tr>");
                emailBody.AppendLine("<td style=\"vertical-align: top;\">");
                // Shipping
                emailBody.AppendLine("<div>");
                emailBody.AppendLine("<h3 style=\"font-weight: bold; color: black;\">Shipping Address</h3>");
                emailBody.AppendLine($"<p style=\"margin-bottom: 0;\">{userEmailClaim}</p>");
                emailBody.AppendLine("<p style=\"margin-bottom: 0; margin-top: 0; color: black;\">XXXX X XXXX XXX,</p>");
                emailBody.AppendLine("<p style=\"margin-top: 0; color: black;\">XXXXXX XX XXXXXX</p>");
                emailBody.AppendLine("<h3 style=\"font-weight: bold; color: black;\">Payment</h3>");
                emailBody.AppendLine("<p style=\"color: black;\">Capital One - XXXX XXXX</p>");
                emailBody.AppendLine("</div>");
                emailBody.AppendLine("</td>");

                emailBody.AppendLine("<td style=\"vertical-align: top; width:20%;\">");
                // Details
                emailBody.AppendLine("<div>");
                emailBody.AppendLine("<h3 style=\"font-weight: bold; color: black;\">Details</h3>");
                emailBody.AppendLine($"<p style=\"font-weight: bold; color: black;\">Total Price:</p>");
                emailBody.AppendLine($"<p style=\"font-weight: bold; color: black;\">Tax:</p>");
                emailBody.AppendLine("</div>");
                emailBody.AppendLine("</td>");

                // Price
                emailBody.AppendLine("<td style=\"vertical-align: top;\">");
                emailBody.AppendLine("<div>");
                emailBody.AppendLine("<br />");
                emailBody.AppendLine("<br />");
                emailBody.AppendLine($"<p style=\"margin-bottom: 17; margin-top: 14; color: black;\">${model.TotalPrice:0.00}</p>");
                emailBody.AppendLine($"<p style=\"color: black;\">${model.TotalTax:0.00}</p>");
                emailBody.AppendLine("</div>");
                emailBody.AppendLine("</td>");
                emailBody.AppendLine("</tr>");
                emailBody.AppendLine("</table>");
                emailBody.AppendLine("<div style=\"clear: both;\"></div>");

                emailBody.AppendLine("<hr style=\"border: 1px solid #ccc;\" />");
               
                emailBody.AppendLine($"<p style=\"text-align: center; font-size: 13px; margin-bottom: 0; font-weight: bold; color: black;\">Order Total</p>");
                emailBody.AppendLine($"<p style=\"text-align: center; font-size: 30px; font-weight: bold; margin-top: 0; color: black;\">${model.FinalPrice.ToString("0.00")}</p>");
                emailBody.AppendLine("<hr style=\"border: 1px solid #ccc;\" />");
                emailBody.AppendLine("<p style=\"text-align: center; font-weight: bold; font-size: 21px; margin-bottom: 12; color: black;\">Keep in Touch");
                emailBody.AppendLine($"<p style=\"text-align: center; font-weight: bold; color: black;\">If you have any questions, concerns, or suggestions,<br /> please do not reply to this email. </p>");
                emailBody.AppendLine("</div>");


                var apiKey = _configuration["ApiKey"];
                var fromEmail = _configuration["FromEmail"];

                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage
                {
                    From = new EmailAddress(fromEmail),
                    Subject = "Order Receipt",
                    HtmlContent = emailBody.ToString(),
                };
                msg.AddTo(new EmailAddress(userEmailClaim));

                await client.SendEmailAsync(msg);

                _context.CartItems.RemoveRange(cart.Items);
                _context.SaveChanges();

                TempData["Message"] = "Thank you for shopping with us!";
            }

            return RedirectToAction("Summary", "Cart");
        }

        public IActionResult ShoppingAction()
        {
            return RedirectToAction("Index", "ProductCatalog");
        }
    }
}