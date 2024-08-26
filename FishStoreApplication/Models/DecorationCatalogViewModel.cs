namespace FishStoreApplication.Models
{
    public class DecorationCatalogViewModel
    {
        public DecorationCatalogViewModel(List<Decoration> decorations, int lastPage, int currPage)
        {
            Decorations = decorations;
            LastPage = lastPage;
            CurrentPage = currPage;

        }
        /// <summary>
        /// Gets or sets the list of decorations in the catalog.
        /// </summary>
        public List<Decoration> Decorations { get; set; }
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
