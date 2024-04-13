using System.ComponentModel.DataAnnotations;

namespace FishStoreApplication.Models
{
    /// <summary>
    /// Represents a single fish for available for purchase
    /// </summary>
    public class Fish
    {
        /// <summary>
        /// The unique identifier for each fish product
        /// (Primary Key)
        /// </summary>
        [Key]
        public int FishId { get; set; }
        /// <summary>
        /// Represents the name of the fish breed.
        /// </summary>
        [Required]
        public string BreedName { get; set; }

        /// <summary>
        /// Represents the price of the fish.
        /// </summary>
        [Range(0, int.MaxValue)]
        public double Price { get; set; }

        /// <summary>
        /// Represents a description of the fish.
        /// </summary>
        public string Description { get; set; }
    }
}
