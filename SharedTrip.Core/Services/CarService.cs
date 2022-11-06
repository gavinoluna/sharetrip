using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using SharedTrip.Infrastructure.Data;

namespace SharedTrip.Core.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext context;

        public CarService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ProfileCarViewModel>> GetMyCarsAsync(string userId)
        {
            return await this.context
                .Cars
                .Where(c => c.DriverId == userId)
                .Select(c => new ProfileCarViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand.Name,
                    Colour = c.Colour.Name,
                    ImageUrl = c.ImageUrl,
                    Model = c.Model,
                    Year = c.Year
                })
                .ToListAsync();
        }
    }
}