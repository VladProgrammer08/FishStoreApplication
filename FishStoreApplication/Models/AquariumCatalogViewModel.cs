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

        public List<Aquarium> Aquariums { get; set; }

        public int LastPage { get; set; }

        public int CurrentPage { get; set; }
    }
}
