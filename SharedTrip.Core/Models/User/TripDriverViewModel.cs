namespace SharedTrip.Core.Models.User
{
    public class TripDriverViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public int CountOfTripsAsDriver { get; set; }

        public int CountOfTripsAsPassenger { get; set; }

        public string ProfileImageUrl { get; set; } = null!;
    }
}