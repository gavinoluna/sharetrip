using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Models.Comments;

namespace SharedTrip.Core.Models.User
{
    public class ProfileViewModel
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string ProfileImageUrl { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int CountOfTripsAsDriver { get; set; }

        public int CountOfTripsAsPassenger { get; set; }

        public double Rating { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}