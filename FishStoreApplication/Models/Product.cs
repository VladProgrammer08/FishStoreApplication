namespace FishStoreApplication.Models
{
    public class Product
    {
        /// <summary>
        /// A unique Id for every product
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// A unique name for every product
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A price of each product
        /// </summary>
        public double Price { get; set; }
    }
}
