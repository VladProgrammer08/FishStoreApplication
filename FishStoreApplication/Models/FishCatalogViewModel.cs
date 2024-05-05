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

        public List<Fish> Fishes { get; set; }
        
        public int LastPage { get; set; }

        public int CurrentPage { get; set; }
    }
}
