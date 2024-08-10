namespace FishStoreApplication.Models
{
    public class Decoration : Product
    {
        /// <summary>
		/// A primary color of the decoration
		/// </summary>
        public string Color { get; set; }
        /// <summary>
		/// Material from which the decoration is made
		/// </summary>
        public string Material { get; set; }
        /// <summary>
		/// A weight of the decoration
		/// </summary>
        public double DecorationWeight { get; set; }
    }

    public class DecorationFileUpload
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

    public  class DecorationViewModel
    {
        /// <summary>
        /// Makes a navigation between Decoration and DecorationFileUpload classes
        /// </summary>
        public Decoration Decoration { get; set; }
        /// <summary>
		/// Makes a navigation between DecorationFileUpload and Decoration classes
		/// </summary>
        public DecorationFileUpload DecorationFileUpload { get; set; }
    }
}
