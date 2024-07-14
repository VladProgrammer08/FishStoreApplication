using System.ComponentModel.DataAnnotations;

namespace FishStoreApplication.Models
{
	public class WishList
	{
		/// <summary>
		/// The unique identifier for each wish list
		/// (Primary Key)
		/// </summary>
		[Key]
		public int WishListId { get; set; }

		/// <summary>
		/// A unique id for every user
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Gets a read-only collection of WishListItem objects.
		/// </summary>
		public virtual ICollection<WishListItem> Items { get; set; }

		/// <summary>
		/// If WishList equals to null, make a new one
		/// </summary>
		public WishList()
		{
			Items = new List<WishListItem>();
		}
	}
	public class WishListItem
	{
		[Key]
		public int WishListItemId { get; set; }

		/// <summary>
		/// The unique identifier for each fish product
		/// </summary>
		public int FishId { get; set; }

		/// <summary>
		/// The unique identifier for each wish list
		/// </summary>
		public int WishListId { get; set; }

		/// <summary>
		/// Navigation property to the Fish entity
		/// </summary>
		public virtual Fish Fish { get; set; }

		/// <summary>
		/// Navigation property back to the WishList
		/// </summary>
		public virtual WishList WishList { get; set; }
	}
}
