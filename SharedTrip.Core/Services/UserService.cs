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
        private readonly ICloudinaryService cloudinaryService;

        public UserService(
            ApplicationDbContext context,
            ICommentService commentService,
            ICarService carService,
            ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.commentService = commentService;
            this.carService = carService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<bool> EditUserAsync(EditUserViewModel model)
        {
            var isEdited = false;

            var user = await this.context
                .Users
                .Where(u => u.Id == model.Id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return isEdited;
            }

            try
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.ProfilePictureUrl = await this.cloudinaryService.UploadPicture(model.ProfilePicture);

                await this.context.SaveChangesAsync();
                isEdited = true;
            }
            catch (Exception)
            {
                isEdited = false;
            }

            return isEdited;
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
                    CountOfTripsAsPassenger = this.context.Trips.Where(t => t.PassengersTrips.Any(pt => pt.PassengerId == u.Id) && t.IsDeleted == false).Count(),
                    CountOfTripsAsDriver = this.context.Trips.Where(t => t.DriverId == u.Id && t.IsDeleted == false).Count()
                })
                .FirstOrDefaultAsync();

            if (showMoreInfo)
            {
                user.Cars = await this.carService.GetMyCarsAsync(userId);
            }

            //user.Comments = await this.commentService.GetAllByCommentsByUserIdAsync(userId);

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
                    CountOfTripsAsPassenger = this.context.Trips.Where(t => t.PassengersTrips.Any(pt => pt.PassengerId == u.Id) && t.IsDeleted == false).Count(),
                    CountOfTripsAsDriver = this.context.Trips.Where(t => t.DriverId == u.Id && t.IsDeleted == false).Count(),
                    Name = $"{u.FirstName} {u.LastName}",
                    PhoneNumber = u.PhoneNumber,
                    ProfileImageUrl = u.ProfilePictureUrl
                })
                .FirstOrDefaultAsync();

            return driver;
        }

        public async Task<EditUserViewModel> GetUserForEditAsync(string userId)
        {
            var user = await this.context
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new EditUserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber
                })
                .FirstOrDefaultAsync();

            return user;
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