using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Models.User;

namespace SharedTrip.Core.Models.Trip
{
	public class TripDetailsViewModel
	{
        public TripViewModel Trip { get; set; } = null!;

        public CarDetailsViewModel Car { get; set; } = null!;

        public TripDriverViewModel Driver { get; set; } = null!;
    }
}