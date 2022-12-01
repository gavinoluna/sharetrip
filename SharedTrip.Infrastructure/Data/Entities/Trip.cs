using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.Trip;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class Trip
    {
        public int Id { get; set; }

        [Required]
        public string DriverId { get; set; } = null!;

        [ForeignKey(nameof(DriverId))]
        public ApplicationUser Driver { get; set; } = null!;

        public int CarId { get; set; }

        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; } = null!;

        public decimal PricePerPerson { get; set; }

        public int StartDestinationId { get; set; }

        [ForeignKey(nameof(StartDestinationId))]
        public PopulatedPlace StartDestination { get; set; } = null!;

        public int EndDestinationId { get; set; }

        [ForeignKey(nameof(EndDestinationId))]
        public PopulatedPlace EndDestination { get; set; } = null!;

        public DateTime Date { get; set; }

        public bool SpaceForLuggage { get; set; }

        public bool AllowedBaverages { get; set; }

        public bool AllowedFood { get; set; }

        public bool AllowedSmoking { get; set; }

        [StringLength(AdditionalInformationMaxLength)]
        public string? AdditionalInformation { get; set; }

        public List<PassengerTrip> PassengersTrips { get; set; } = new List<PassengerTrip>();

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public int CountOfSeats { get; set; }
    }
}