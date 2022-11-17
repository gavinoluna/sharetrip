using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.User;
using SharedTrip.Infrastructure.Data;

namespace SharedTrip.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly ICommentService commentService;
        private readonly ICarService carService;

        public UserService(
            ApplicationDbContext context,
            ICommentService commentService,
            ICarService carService)
        {
            this.context = context;
            this.commentService = commentService;
            this.carService = carService;
        }

        public async Task<int> GetCountOfUsersAsync()
            => await this.context
                         .Users
                         .CountAsync();

        public async Task<ProfileViewModel> GetProfileInfoAsync(string userId, bool showMoreInfo)
        {
            var user = await this.context
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new ProfileViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfileImageUrl = u.ProfilePictureUrl,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    Rating = u.Rating,
                    CountOfTripsAsPassenger = this.context.Trips.Where(t => t.PassengersTrips.Any(pt => pt.PassengerId == u.Id)).Count(),
                    CountOfTripsAsDriver = this.context.Trips.Where(t => t.DriverId == u.Id).Count()
                })
                .FirstOrDefaultAsync();

            if (showMoreInfo)
            {
                user.Cars = await this.carService.GetMyCarsAsync(userId);
            }

            user.Comments = await this.commentService.GetAllByCommentsByUserIdAsync(userId);

            return user;
        }

        public async Task<TripDriverViewModel> GetTripDriverAsync(string driverId)
        {
            var driver = await this.context
                .Users
                .Where(u => u.Id == driverId)
                .Select(u => new TripDriverViewModel
                {
                    Id = u.Id,
                    CountOfTripsAsDriver = u.CountOfTripsAsDriver,
                    CountOfTripsAsPassenger = u.CountOfTripsAsPassenger,
                    Name = $"{u.FirstName} {u.LastName}",
                    PhoneNumber = u.PhoneNumber,
                    ProfileImageUrl = u.ProfilePictureUrl
                })
                .FirstOrDefaultAsync();

            return driver;
        }

        public async Task<bool> HasCar(string userId)
        {
            var hasCar = false;

            var user = await this.context
                .Users
                .Include(u => u.Cars)
                .FirstAsync(u => u.Id == userId);

            if (user.Cars.Any())
            {
                hasCar = true;
            }

            return hasCar;
        }
    }
}