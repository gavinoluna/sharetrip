namespace SharedTrip.Core.Models.Trip.ServiceModels
{
    public class MyTripQueryModel
    {
        public const int TripsPerPage = 5;

        public int CurrentPage { get; set; } = 1;

        public int TotalTripsCount { get; set; }

        public IEnumerable<AllTripsViewModel> Trips { get; set; } = Enumerable.Empty<AllTripsViewModel>();
    }
}