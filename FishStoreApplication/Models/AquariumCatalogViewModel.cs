namespace FishStoreApplication.Models
{
    public class AquariumCatalogViewModel
    {
        public AquariumCatalogViewModel(List<Aquarium> aquariums, int lastPage, int currPage)
        {
            Aquariums = aquariums;
            LastPage = lastPage;
            CurrentPage = currPage;
        }
        /// <summary>
        /// Gets or sets the list of aquarium in the catalog.
        /// </summary>
        public List<Aquarium> Aquariums { get; set; }
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
