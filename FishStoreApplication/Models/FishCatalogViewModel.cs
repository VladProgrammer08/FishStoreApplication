namespace FishStoreApplication.Models
{
    public class FishCatalogViewModel
    {
        public FishCatalogViewModel(List<Fish> fishes, int lastPage, int currPage)
        {
            Fishes = fishes;
            LastPage = lastPage;
            CurrentPage = currPage;
        }
        /// <summary>
        /// Gets or sets the list of fishes in the catalog.
        /// </summary>
        public List<Fish> Fishes { get; set; }
        /// <summary>
        /// Gets or sets the last page number.
        /// </summary>
        public int LastPage { get; set; }
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
