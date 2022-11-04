using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class PassengerTrip
    {
        [Required]
        public string PassengerId { get; set; } = null!;

        [ForeignKey(nameof(PassengerId))]
        public ApplicationUser Passenger { get; set; } = null!;

        public int TripId { get; set; }

        [ForeignKey(nameof(TripId))]
        public Trip Trip { get; set; } = null!;
    }
}