using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Models.User;

namespace SharedTrip.Core.Models.Trip
{
    public class TripViewModel
    {
        public int Id { get; set; }

        public string DriverId { get; set; } = null!;

        public int CarId { get; set; }

        public string StartDestination { get; set; } = null!;

        public string EndDestination { get; set; } = null!;

        public decimal Price { get; set; }

        public string Date { get; set; } = null!;

        public int FreeSeats { get; set; }

        public int AllSeats { get; set; }

        public bool IsActive { get; set; }

        public bool SpaceForLuggage { get; set; }

        public bool AllowedBaverages { get; set; }

        public bool AllowedFood { get; set; }

        public bool AllowedSmoking { get; set; }

        public string AdditionalInformation { get; set; } = null!;

        public IEnumerable<PassengerViewModel> Passengers { get; set; } = new List<PassengerViewModel>();
    }
}