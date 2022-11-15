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

        public async Task<IEnumerable<MyTripViewModel>> GetMyTripsAsync(string userId)
        {
            var trips = await this.context
                .Trips
                .Where(t => (t.DriverId == userId || t.PassengersTrips.Any(pt => pt.PassengerId == userId)) && t.IsActive == true)
                .Select(t => new MyTripViewModel
                {
                    Id = t.Id,
                    StartDestination = t.StartDestination.Name,
                    EndDestination = t.EndDestination.Name,
                    DriverId = t.DriverId,
                    DriverImageUrl = t.Driver.ProfilePictureUrl,
                    DriverName = $"{t.Driver.FirstName} {t.Driver.LastName}",
                    Car = $"{t.Car.Brand.Name} {t.Car.Model}",
                    Price = t.PricePerPerson,
                    IsActive = t.IsActive,
                    AllSeats = t.Car.CountOfSeats,
                    FreeSeats = t.Car.CountOfSeats - (t.PassengersTrips.Count() + 1),
                    Date = t.Date.ToString("MM/dd/yyyy HH:mm")
                })
                .ToListAsync();

            return trips;
        }

        public async Task<TripViewModel> GetTripDetailsAsync(int tripId)
        {
            var trip = await this.context
                .Trips
                .Where(t => t.Id == tripId)
                .Select(t => new TripViewModel
                {
                    Id = t.Id,
                    StartDestination = t.StartDestination.Name,
                    EndDestination = t.EndDestination.Name,
                    DriverId = t.DriverId,
                    CarId = t.CarId,
                    Price = t.PricePerPerson,
                    IsActive = t.IsActive,
                    AllSeats = t.Car.CountOfSeats,
                    FreeSeats = t.Car.CountOfSeats - (t.PassengersTrips.Count() + 1),
                    Date = t.Date.ToString("MM/dd/yyyy HH:mm"),
                    AdditionalInformation = t.AdditionalInformation,
                    AllowedBaverages = t.AllowedBaverages,
                    AllowedFood = t.AllowedFood,
                    AllowedSmoking = t.AllowedSmoking,
                    SpaceForLuggage = t.SpaceForLuggage
                })
                .FirstOrDefaultAsync();

            return trip;
        }
    }
}