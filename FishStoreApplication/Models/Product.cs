using System.ComponentModel.DataAnnotations;

namespace FishStoreApplication.Models
{
    public class Product
    {
        /// <summary>
        /// The unique identifier for each product
        /// (Primary Key)
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// A unique name for a single product
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A price of a single product
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// A main image for a single product
        /// </summary>
        public string MainImageURL { get; set; }
        /// <summary>
        /// A length of the product
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// A height of the product
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// A width of the product
        /// </summary>
        public int Width { get; set; }
        /// <summary>
		/// Represents the URL of the secondary product image one.
		/// </summary>
		public string? SecondaryImageOne { get; set; }
        /// <summary>
        /// Represents the URL of the secondary product image two.
        /// </summary>
        public string? SecondaryImageTwo { get; set; }
        /// <summary>
        /// Represents the URL of the secondary product image three.
        /// </summary>
        public string? SecondaryImageThree { get; set; }
        /// <summary>
        /// Represents the URL of the secondary product image four.
        /// </summary>
        public string? SecondaryImageFour { get; set; }
        /// <summary>
        /// Represents a description of the product.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
		/// Specifies any warranty information related to the product.
		/// </summary>
		public string? Warranty { get; set; }
    }
}
