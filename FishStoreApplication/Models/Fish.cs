using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;

namespace FishStoreApplication.Models
{
    /// <summary>
    /// Represents a single fish for available for purchase
    /// </summary>
    public class Fish : Product
    {
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
    }

    public class CreateProductViewModel
    {
        public IFormFile MainImage { get; set; }
        public IFormFile SecondaryImageOne { get; set; }
        public IFormFile SecondaryImageTwo { get; set; }
        public IFormFile SecondaryImageThree { get; set; }
        public IFormFile SecondaryImageFour { get; set; }
    }

    public class FishViewModel
    {
        public Fish Fish { get; set; }

        public CreateProductViewModel FishUpload { get; set; }
    }
}
