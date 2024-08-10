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
        public List<Decoration> Decorations { get; set; }

        public int LastPage { get; set; }

        public int CurrentPage { get; set; }
    }
}
