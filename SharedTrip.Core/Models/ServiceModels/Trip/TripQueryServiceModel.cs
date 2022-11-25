namespace SharedTrip.Core.Models.Trip.ServiceModels
{
    public class TripQueryServiceModel
    {
        public int TotalTripsCount { get; set; }

        public IEnumerable<AllTripsViewModel> Trips { get; set; } = new List<AllTripsViewModel>();
    }
}