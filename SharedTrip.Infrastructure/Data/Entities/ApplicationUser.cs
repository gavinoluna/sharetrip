using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.User;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string ProfilePictureUrl { get; set; } = null!;

        public int CountOfTripsAsDriver { get; set; }

        public int CountOfTripsAsPassenger { get; set; }

        public double Rating { get; set; }

        public IEnumerable<Car> Cars { get; set; } = new List<Car>();

        public IEnumerable<Trip> DriverTrips { get; set; } = new List<Trip>();

        public IEnumerable<PassengerTrip> PassengersTrips { get; set; } = new List<PassengerTrip>();

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<Message> SentMessages { get; set; } = new List<Message>();

        public IEnumerable<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}