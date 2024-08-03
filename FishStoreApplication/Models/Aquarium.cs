using System.ComponentModel.DataAnnotations;

namespace FishStoreApplication.Models
{
	public class Aquarium : Product
	{
		/// <summary>
		/// Unique identifier for each aquarium.
		/// </summary>
		[Key]
        [Display(Name = "Aquarium ID")]
        public int AquariumId { get; set; }
		/// <summary>
		/// Capacity of the aquarium in gallons.
		/// </summary>
		public double AquariumGallons { get; set; }
		/// <summary>
		/// Cost of the aquarium.
		/// </summary>
		[Range(0, int.MaxValue)]
		[Display(Name = "Price")]
		public double AquariumPrice { get; set; }
		/// <summary>
		/// Brand name of the aquarium.
		/// </summary>

		[Display(Name = "Brand")]
		public string AquariumBrand { get; set; }
		/// <summary>
		/// Material from which the aquarium is made
		/// </summary>
		public string AquariumMaterial { get; set; }
		/// <summary>
		/// Represents the URL of the main aquarium image.
		/// </summary>
		public string? MainImageURL { get; set; }
		/// <summary>
		/// Represents the URL of the secondary aquarium image one.
		/// </summary>
		public string? SecondaryImageOne { get; set; }
		/// <summary>
		/// Represents the URL of the secondary aquarium image two.
		/// </summary>
		public string? SecondaryImageTwo { get; set; }
		/// <summary>
		/// Represents the URL of the secondary aquarium image three.
		/// </summary>
		public string? SecondaryImageThree { get; set; }
		/// <summary>
		/// Represents the URL of the secondary aquarium image four.
		/// </summary>
		public string? SecondaryImageFour { get; set; }
		/// <summary>
		/// Represents a description of the aquarium.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Specifies any warranty information related to the aquarium.
		/// </summary>
		public string? AquariumWarranty { get; set; }
	}

	public class AquariumFileUpload
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

	public class AquariumViewModel
	{
		/// <summary>
		/// Makes a navigation between Aquarium and AquariumFileUpload classes
		/// </summary>
		public Aquarium Aquarium { get; set; }
		/// <summary>
		/// Makes a navigation between AquariumFileUpload and Aquarium classes
		/// </summary>
		public AquariumFileUpload AquariumFileUpload { get; set;}
	}
}
