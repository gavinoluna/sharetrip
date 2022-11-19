namespace SharedTrip.Core.Models.Trip
{
    public class TripQueryServiceModel
    {
        public int TotalTripsCount { get; set; }

        public IEnumerable<AllTripsViewModel> Trips { get; set; } = new List<AllTripsViewModel>();
    }
}