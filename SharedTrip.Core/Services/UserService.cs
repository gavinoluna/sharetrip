using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.ServiceModels.User;
using SharedTrip.Core.Models.User;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public UserService(
            ApplicationDbContext context,
            ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task EditUserAsync(EditUserViewModel model)
        {
            var user = await this.context
                .Users
                .Where(u => u.Id == model.Id)
                .FirstOrDefaultAsync();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.ProfilePictureUrl = await this.cloudinaryService.UploadPicture(model.ProfilePicture);

            await this.context.SaveChangesAsync();
        }

        public async Task<int> GetCountOfUsersAsync()
            => await this.context
                         .Users
                         .CountAsync();

        public async Task<ProfileViewModel> GetProfileInfoAsync(string userId)
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

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await this.context
                .Users
                .FindAsync(userId);
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

        public async Task<UserQueryServiceModel> GetUsers(int currentPage = 1, int usersPerPage = 6)
        {
            var model = new UserQueryServiceModel();

            var usersQuery = this.context
                .Users
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .AsQueryable();

            var users = await usersQuery
                .Skip((currentPage - 1) * usersPerPage)
                .Take(usersPerPage)
                .Select(u => new UserListViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                })
                .ToListAsync();

            model.Users = users;
            model.TotalUsersCount = await usersQuery.CountAsync();

            return model;
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