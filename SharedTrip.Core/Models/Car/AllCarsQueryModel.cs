namespace SharedTrip.Core.Models.Car
{
    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 5;

        public int CurrentPage { get; set; } = 1;

        public int TotalCarsCount { get; set; }

        public IEnumerable<ProfileCarViewModel> Cars { get; set; } = Enumerable.Empty<ProfileCarViewModel>();
    }
}