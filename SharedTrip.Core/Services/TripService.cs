using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Trip;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.Core.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext context;

        public TripService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CreateTripAsync(CreateTripViewModel model, string driverId)
        {
            var trip = new Trip
            {
                AdditionalInformation = model.AdditionalInformation,
                AllowedBaverages = model.AllowedBaverages,
                AllowedFood = model.AllowedFood,
                AllowedSmoking = model.AllowedSmoking,
                CarId = model.CarId,
                Date = model.Date,
                PricePerPerson = model.PricePerPerson,
                StartDestinationId = model.StartDestinationId,
                EndDestinationId = model.EndDestinationId,
                SpaceForLuggage = model.SpaceForLuggage,
                DriverId = driverId
            };

            try
            {
                await this.context.Trips.AddAsync(trip);
                await this.context.SaveChangesAsync();
                return trip.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> GetCountOfTripsAsync()
            => await this.context
                         .Trips
                         .CountAsync();

        public async Task<IEnumerable<PopulatedPlaceViewModel>> GetPopulatedPlacesAsync()
        {
            return await this.context
                .PopulatedPlaces
                .Select(p => new PopulatedPlaceViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<bool> CheckWhetherUserIsFree(string userId, CreateTripViewModel tripModel)
        {
            var user = await this.context.Users
                .Include(u => u.DriverTrips)
                .Include(u => u.PassengersTrips)
                .FirstAsync(u => u.Id == userId);

            if (user.PassengersTrips.Any(pt => DateTime.Compare(pt.Trip.Date, tripModel.Date) == 0)
                || user.DriverTrips.Any(dt => DateTime.Compare(dt.Date, tripModel.Date) == 0))
            {
                return false;
            }

            return true;
        }
    }
}