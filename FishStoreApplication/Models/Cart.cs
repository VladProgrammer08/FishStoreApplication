using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishStoreApplication.Models
{
    public class Cart
    {
        /// <summary>
        /// The unique identifier for each product
        /// (Primary Key)
        /// </summary>
        [Key]
        public int CartId { get; set; }

        /// <summary>
        /// A unique id for every user
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets a read-only collection of CartItem objects.
        /// </summary>
        public virtual ICollection<CartItem> Items { get; set; }
        /// <summary>
        /// If Cart equals to null, make a new one
        /// </summary>
        public Cart()
        {
            Items = new List<CartItem>();
        }
    }

    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        /// <summary>
        /// The unique identifier for each product
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Shows a quantity of an item
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The unique identifier for each product
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Navigation property to the Product
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Navigation property back to the Cart
        /// </summary>
        public virtual Cart Cart { get; set; }
    }

    
    public class CartSummaryViewModel
    {
        /// <summary>
        /// Gets a collection of CartItem objects.
        /// </summary>
        public List<CartItem> Items { get; set; }

        /// <summary>
        /// A total price of all the cart items
        /// </summary>
        public double TotalPrice { get; set; }

        /// <summary>
        /// Property to hold the tax rate
        /// </summary>
        public double TaxRate { get; set; }

        /// <summary>
        /// Property to calculate the total tax based on the TotalPrice and TaxRate
        /// </summary>
        public double TotalTax => TotalPrice * TaxRate;

        /// <summary>
        /// Property to calculate the final price including tax
        /// </summary>
        public double FinalPrice => TotalPrice + TotalTax;

    }


}
