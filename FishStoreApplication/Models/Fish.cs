using Microsoft.Build.Execution;
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
        [Display(Name = "Breed Name")]
        public string BreedName { get; set; }

        /// <summary>
        /// Represents the URL of the main fish image.
        /// </summary>
        public string? MainImageURL { get; set; }
        /// <summary>
        /// Represents the URL of the secondary fish image one.
        /// </summary>
        public string? SecondaryImageOne { get; set; }
        /// <summary>
        /// Represents the URL of the secondary fish image two.
        /// </summary>
        public string? SecondaryImageTwo { get; set; }
        /// <summary>
        /// Represents the URL of the secondary fish image three.
        /// </summary>
        public string? SecondaryImageThree { get; set; }
        /// <summary>
        /// Represents the URL of the secondary fish image four.
        /// </summary>
        public string? SecondaryImageFour { get; set; }
        /// <summary>
        /// Represents the price of the fish.
        /// </summary>
        [Range(0, int.MaxValue)]
        public double Price { get; set; }

        /// <summary>
        /// Represents the size description of the fish.
        /// </summary>
        public int? FishSize { get; set; }

        /// <summary>
        /// Represents any special dietary requirements for the fish.
        /// </summary>
        public string? FishSpecialDiet { get; set; }

        /// <summary>
        /// Describes the preferred environment or habitat for the fish.
        /// </summary>
        public string? FishEnvironment { get; set; }

        /// <summary>
        /// Indicates the temperament or behavior traits of the fish.
        /// </summary>
        public string? FishTemperament { get; set; }

        /// <summary>
        /// Represents the care level required for maintaining the fish.
        /// </summary>
        public string? FishCareLevel { get; set; }

        /// <summary>
        /// Represents a description of the fish.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Specifies any warranty information related to the fish.
        /// </summary>
        public string? FishWarranty { get; set; }
    }
}
