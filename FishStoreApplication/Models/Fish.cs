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
        /// <summary>
		/// The main image file to be uploaded.
		/// </summary>
        public IFormFile MainImage { get; set; }
        /// <summary>
		/// The secondary image file to be uploaded.
		/// </summary>
        public IFormFile SecondaryImageOne { get; set; }
        /// <summary>
		/// The secondary image file to be uploaded.
		/// </summary>
        public IFormFile SecondaryImageTwo { get; set; }
        /// <summary>
		/// The secondary image file to be uploaded.
		/// </summary>
        public IFormFile SecondaryImageThree { get; set; }
        /// <summary>
		/// The secondary image file to be uploaded.
		/// </summary>
        public IFormFile SecondaryImageFour { get; set; }
    }

    public class FishViewModel
    {
        /// <summary>
		/// Makes a navigation between Fish and FileUpload classes
		/// </summary>
        public Fish Fish { get; set; }
        /// <summary>
		/// Makes a navigation between FileUpload and Fish classes
		/// </summary>
        public CreateProductViewModel FishUpload { get; set; }
    }
}
