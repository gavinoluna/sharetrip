using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.Core.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext context;
        private readonly ICloudinaryService cloudinaryService;

        public CarService(
            ApplicationDbContext context,
            ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<bool> AddCarAsync(AddCarViewModel model, string userId)
        {
            var isCreated = false;
            var user = await this.context.Users.FindAsync(userId);

            if (user != null)
            {
                var car = new Car
                {
                    BrandId = model.BrandId,
                    ColourId = model.ColourId,
                    Climatronic = model.Climatronic,
                    CountOfSeats = model.CountOfSeats,
                    DriverId = userId,
                    Year = model.Year
                };

                try
                {
                    car.ImageUrl = await this.cloudinaryService.UploadPicture(model.Image);
                    await this.context.Cars.AddAsync(car);
                    await this.context.SaveChangesAsync();
                    isCreated = true;
                }
                catch (Exception)
                {
                    isCreated = false;
                }
            }

            return isCreated;
        }

        public async Task<IEnumerable<BrandViewModel>> GetBrandsAsync()
            => await this.context
                         .Brands
                         .Select(b => new BrandViewModel
                         {
                             Id = b.Id,
                             Name = b.Name
                         })
                         .ToListAsync();

        public async Task<IEnumerable<ColourViewModel>> GetColoursAsync()
            => await this.context
                         .Colours
                         .Select(b => new ColourViewModel
                         {
                             Id = b.Id,
                             Name = b.Name
                         })
                         .ToListAsync();

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