using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Models.Comments;
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
                    CountOfTripsAsPassenger = u.CountOfTripsAsPassenger,
                    CountOfTripsAsDriver = u.CountOfTripsAsDriver
                })
                .FirstOrDefaultAsync();

            if (showMoreInfo)
            {
                user.Cars = await this.carService.GetMyCarsAsync(userId);
            }

            user.Comments = await this.commentService.GetAllByCommentsByUserIdAsync(userId);

            return user;
        }
    }
}