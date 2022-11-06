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

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<MyProfileViewModel> GetMyProfileDataAsync(string userId)
        {
            var user = await this.context
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new MyProfileViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfileImageUrl = u.ProfilePictureUrl,
                    PhoneNumber = u.PhoneNumber,
                    Rating = u.Rating,
                    CountOfTripsAsPassenger = u.CountOfTripsAsPassenger,
                    CountOfTripsAsDriver = u.CountOfTripsAsDriver,
                    Cars = u.Cars.Select(c => new ProfileCarViewModel
                    {
                        Id = c.Id,
                        Brand = c.Brand.Name,
                        Colour = c.Colour.Name,
                        ImageUrl = c.ImageUrl,
                        Model = c.Model,
                        Year = c.Year
                    })
                    .ToList(),
                    Comments = u.ReceivedComments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatorName = $"{c.Creator.FirstName} {c.Creator.LastName}",
                        CreatedOn = c.CreatedOn.ToString(),
                        CreatorId = c.CreatorId
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return user;
        }
    }
}