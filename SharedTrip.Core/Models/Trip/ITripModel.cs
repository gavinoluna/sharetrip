using SharedTrip.Core.CustomAttributes;
using SharedTrip.Core.Models.Car;

namespace SharedTrip.Core.Models.Trip
{
    public interface ITripModel
    {
        public int CarId { get; set; }

        public IEnumerable<CreateTripCarViewModel> Cars { get; set; }

        public decimal PricePerPerson { get; set; }

        public int StartDestinationId { get; set; }

        public IEnumerable<PopulatedPlaceViewModel> StartDestinations { get; set; }

        public int EndDestinationId { get; set; }

        public IEnumerable<PopulatedPlaceViewModel> EndDestinations { get; set; }

        [TripDate]
        public DateTime Date { get; set; }

        public bool SpaceForLuggage { get; set; }

        public bool AllowedBaverages { get; set; }

        public bool AllowedFood { get; set; }

        public bool AllowedSmoking { get; set; }

        public string? AdditionalInformation { get; set; }

        public int CountOfSeats { get; set; }
    }
}