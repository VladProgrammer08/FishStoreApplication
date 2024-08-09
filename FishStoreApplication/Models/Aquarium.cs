using System.ComponentModel.DataAnnotations;

namespace FishStoreApplication.Models
{
	public class Aquarium : Product
	{
		/// <summary>
		/// Material from which the aquarium is made
		/// </summary>
		public string AquariumMaterial { get; set; }
		/// <summary>
		/// A primary color of the aquarium
		/// </summary>
        public string AquariumColor { get; set; }
		/// <summary>
		/// A volume of the aquarium
		/// </summary>
        public int AquariumVolume { get; set; }
		/// <summary>
		/// A weight of the aquarium
		/// </summary>
        public int AquariumWeight { get; set; }
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
