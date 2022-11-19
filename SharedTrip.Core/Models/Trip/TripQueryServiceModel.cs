namespace SharedTrip.Core.Models.Trip
{
    public class TripQueryServiceModel
    {
        public int TotalTripsCount { get; set; }

        public IEnumerable<MyTripViewModel> Trips { get; set; } = new List<MyTripViewModel>();
    }
}