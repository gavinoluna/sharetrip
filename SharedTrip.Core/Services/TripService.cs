using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Trip;
using SharedTrip.Core.Models.Trip.ServiceModels;
using SharedTrip.Core.Models.User;
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
                DriverId = driverId,
                CountOfSeats = model.CountOfSeats
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

        public async Task<bool> CheckWhetherUserIsFree(string userId, DateTime date, int? tripId = null)
        {
            var user = await this.context.Users
                .Include(u => u.DriverTrips)
                .Include(u => u.PassengersTrips)
                .ThenInclude(pt => pt.Trip)
                .FirstAsync(u => u.Id == userId);

            if (user.PassengersTrips.Any(pt => DateTime.Compare(pt.Trip.Date, date) == 0 && pt.Trip.Id != tripId)
                || user.DriverTrips.Any(dt => DateTime.Compare(dt.Date, date) == 0 && dt.Id != tripId))
            {
                return false;
            }

            return true;
        }

        public async Task<TripQueryServiceModel> GetMyTripsAsync(string userId, int currentPage = 1, int tripsPerPage = 5)
        {
            var result = new TripQueryServiceModel();

            var tripsQuery = this.context
                .Trips
                .Where(t => (t.DriverId == userId || t.PassengersTrips.Any(pt => pt.PassengerId == userId)) && t.IsActive == true)
                .OrderBy(t => t.Date)
                .AsQueryable();

            var trips = await tripsQuery
                .Skip((currentPage - 1) * tripsPerPage)
                .Take(tripsPerPage)
                .Select(t => new AllTripsViewModel
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
                    FreeSeats = t.CountOfSeats - t.PassengersTrips.Count(),
                    Date = t.Date.ToString("MM/dd/yyyy HH:mm")
                })
                .ToListAsync();

            result.Trips = trips;
            result.TotalTripsCount = await tripsQuery.CountAsync();

            return result;
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
                    FreeSeats = t.CountOfSeats - t.PassengersTrips.Count(),
                    Date = t.Date.ToString("MM/dd/yyyy HH:mm"),
                    AdditionalInformation = t.AdditionalInformation,
                    AllowedBaverages = t.AllowedBaverages,
                    AllowedFood = t.AllowedFood,
                    AllowedSmoking = t.AllowedSmoking,
                    SpaceForLuggage = t.SpaceForLuggage,
                    Passengers = t.PassengersTrips.Select(p => new PassengerViewModel
                    {
                        Id = p.PassengerId,
                        Name = $"{p.Passenger.FirstName} {p.Passenger.LastName}",
                        ProfileImageUrl = p.Passenger.ProfilePictureUrl
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return trip;
        }

        public async Task<bool> DeleteTripAsync(int tripId)
        {
            var isDeleted = false;

            var trip = await this.context
                .Trips
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip != null)
            {
                try
                {
                    this.context.Trips.Remove(trip);
                    await this.context.SaveChangesAsync();
                    isDeleted = true;
                }
                catch (Exception)
                {
                    isDeleted = false;
                }
            }

            return isDeleted;
        }

        public async Task<EditTripViewModel> GetTripForEditAsync(int tripId)
        {
            var trip = await this.context
                .Trips
                .Where(t => t.Id == tripId)
                .Select(t => new EditTripViewModel
                {
                    AdditionalInformation = t.AdditionalInformation,
                    AllowedBaverages = t.AllowedBaverages,
                    AllowedFood = t.AllowedFood,
                    AllowedSmoking = t.AllowedSmoking,
                    CarId = t.CarId,
                    Date = t.Date,
                    PricePerPerson = t.PricePerPerson,
                    StartDestinationId = t.StartDestinationId,
                    EndDestinationId = t.EndDestinationId,
                    SpaceForLuggage = t.SpaceForLuggage,
                    DriverId = t.DriverId,
                    CountOfSeats = t.CountOfSeats,
                    Id = t.Id
                })
                .FirstOrDefaultAsync();

            return trip;
        }

        public async Task<bool> EditTripAsync(EditTripViewModel model)
        {
            var isEdited = false;

            var trip = await this.context.Trips.FindAsync(model.Id);

            if (trip == null)
            {
                return isEdited;
            }

            trip.PricePerPerson = model.PricePerPerson;
            trip.CarId = model.CarId;
            trip.StartDestinationId = model.StartDestinationId;
            trip.EndDestinationId = model.EndDestinationId;
            trip.AdditionalInformation = model.AdditionalInformation;
            trip.AllowedBaverages = model.AllowedBaverages;
            trip.AllowedFood = model.AllowedFood;
            trip.AllowedSmoking = model.AllowedSmoking;
            trip.CountOfSeats = model.CountOfSeats;
            trip.Date = model.Date;
            trip.SpaceForLuggage = model.SpaceForLuggage;

            try
            {
                await this.context.SaveChangesAsync();
                isEdited = true;
            }
            catch (Exception)
            {
                isEdited = false;
            }

            return isEdited;
        }

        public async Task<TripQueryServiceModel> AllAsync(
            int startDestinationId,
            int endDestinationId,
            DateTime date,
            int currentPage = 1,
            int tripsPerPage = 5)
        {
            var result = new TripQueryServiceModel();
            var tripsQuery = this.context
                .Trips
                .Where(t => t.IsActive == true)
                .AsQueryable();

            if (startDestinationId > 0 && endDestinationId > 0 && DateTime.Compare(date, DateTime.Now) > 0)
            {
                tripsQuery = tripsQuery
                    .Where(t => t.StartDestinationId == startDestinationId
                    && t.EndDestinationId == endDestinationId
                    && DateTime.Compare(date, t.Date.Date) == 0);
            }

            var trips = await tripsQuery
                .OrderBy(t => t.Date)
                .Skip((currentPage - 1) * tripsPerPage)
                .Take(tripsPerPage)
                .Select(t => new AllTripsViewModel
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
                    FreeSeats = t.CountOfSeats - t.PassengersTrips.Count(),
                    Date = t.Date.ToString("MM/dd/yyyy HH:mm")
                })
                .ToListAsync();

            result.Trips = trips;
            result.TotalTripsCount = await tripsQuery.CountAsync();

            return result;
        }

        public async Task<bool> JoinTripAsync(string userId, int tripId)
        {
            var hasJoined = false;

            var trip = await this.context
                .Trips
                .Where(t => t.Id == tripId)
                .FirstOrDefaultAsync();

            if (trip == null)
            {
                return hasJoined;
            }

            try
            {
                trip.PassengersTrips.Add(new PassengerTrip
                {
                    TripId = tripId,
                    PassengerId = userId
                });

                await this.context.SaveChangesAsync();
                hasJoined = true;
            }
            catch (Exception)
            {
                hasJoined = false;
            }

            return hasJoined;
        }

        public async Task<bool> CheckIfUserIsInTripAsync(string userId, int tripId)
        {
            var isInTrip = false;

            var trip = await this.context
                .Trips
                .Include(t => t.PassengersTrips)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
            {
                return isInTrip;
            }

            if (trip.DriverId == userId || trip.PassengersTrips.Any(pt => pt.PassengerId == userId))
            {
                isInTrip = true;
            }

            return isInTrip;
        }
    }
}