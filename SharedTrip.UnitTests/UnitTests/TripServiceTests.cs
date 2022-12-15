using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Trip;
using SharedTrip.Core.Services;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.UnitTests.UnitTests
{
    public class TripServiceTests : UnitTestsBase
    {
        private ITripService tripService;

        [SetUp]
        public void SetUp()
        {
            this.tripService = new TripService(this.data);
        }

        [Test]
        public async Task CreateTripAsyncMethodShouldCreateTripsCorrectly()
        {
            await this.data.PopulatedPlaces.AddRangeAsync(new List<PopulatedPlace>
            {
                new PopulatedPlace
                {
                    Id = 1,
                    Name = "Sofia"
                },
                new PopulatedPlace
                {
                    Id = 2,
                    Name = "Blagoevgrad"
                },
            });

            await this.data.SaveChangesAsync();

            var newTrip = new CreateTripViewModel
            {
                AdditionalInformation = "Test",
                AllowedBaverages = true,
                AllowedFood = false,
                AllowedSmoking = false,
                CarId = this.Car.Id,
                Date = DateTime.Now,
                PricePerPerson = 10,
                StartDestinationId = 1,
                EndDestinationId = 2,
                SpaceForLuggage = true,
                CountOfSeats = 4
            };

            var tripId = await this.tripService.CreateTripAsync(newTrip, this.User.Id);
            var tripsCount = await this.tripService.GetCountOfTripsAsync();

            Assert.Multiple(() =>
            {
                Assert.That(tripId, Is.Not.EqualTo(0));
                Assert.That(tripsCount, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetPopulatedPlacesAsyncMethodShouldReturnPopulatedPlacesCorrectly()
        {
            await this.data.PopulatedPlaces.AddRangeAsync(new List<PopulatedPlace>
            {
                new PopulatedPlace
                {
                    Id = 1,
                    Name = "Sofia"
                },
                new PopulatedPlace
                {
                    Id = 2,
                    Name = "Blagoevgrad"
                },
            });

            await this.data.SaveChangesAsync();

            var populatedPlaces = await this.tripService.GetPopulatedPlacesAsync();

            Assert.That(populatedPlaces.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task CheckWhetherUserIsFreeMethodShouldReturnTrueIfUserIsFree()
        {
            var result = await this.tripService.CheckWhetherUserIsFree(this.User.Id, DateTime.Now.AddDays(2));

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetCountOfFreeSeatsAsyncMethodShouldReturnFreeSeatsCorrectly()
        {
            var newUser = new ApplicationUser
            {
                Id = "TestId",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            await this.tripService.JoinTripAsync(newUser.Id, 1);
            var freeSeats = await this.tripService.GetCountOfFreeSeatsAsync(1);

            Assert.That(freeSeats, Is.EqualTo(3));
        }

        [Test]
        public async Task TripExistsMethodShouldReturnTrueIfTripExists()
        {
            var tripExists = await this.tripService.TripExists(1);

            Assert.True(tripExists);
        }

        [Test]
        public async Task TripExistsMethodShouldReturnFalseIfTripDoesNotExist()
        {
            var tripExists = await this.tripService.TripExists(20);

            Assert.False(tripExists);
        }

        [Test]
        public async Task IsUserDriverOfTripMethodShouldReturnTrueIfUserIsDriver()
        {
            var isDriver = await this.tripService.IsUserDriverOfTrip(this.User.Id, this.Trip.Id);

            Assert.True(isDriver);
        }

        [Test]
        public async Task IsUserDriverOfTripMethodShouldReturnFalseIfUserIsNotDriver()
        {
            var newUser = new ApplicationUser
            {
                Id = "TestId",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            var isDriver = await this.tripService.IsUserDriverOfTrip(newUser.Id, this.Trip.Id);

            Assert.False(isDriver);
        }

        [Test]
        public async Task CheckIfUserIsInTripAsyncMethodShouldReturnTrueIfUserIsInTrip()
        {
            var isInTrip = await this.tripService.CheckIfUserIsInTripAsync(this.User.Id, this.Trip.Id);

            Assert.True(isInTrip);
        }

        [Test]
        public async Task CheckIfUserIsInTripAsyncMethodShouldReturnFalseIfUserIsNotInTrip()
        {
            var newUser = new ApplicationUser
            {
                Id = "TestId",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            var isInTrip = await this.tripService.CheckIfUserIsInTripAsync(newUser.Id, this.Trip.Id);

            Assert.False(isInTrip);
        }

        [Test]
        public async Task GetTripForEditAsyncMethodShouldReturnTheCorrectTrip()
        {
            var tripForEdit = await this.tripService.GetTripForEditAsync(this.Trip.Id);

            Assert.That(tripForEdit.Id, Is.EqualTo(this.Trip.Id));
        }

        [Test]
        public async Task EditTripAsyncMethodShouldEditTripCorrectly()
        {
            var tripForEdit = await this.tripService.GetTripForEditAsync(this.Trip.Id);

            tripForEdit.AllowedSmoking = true;
            tripForEdit.CountOfSeats = 1;

            await this.tripService.EditTripAsync(tripForEdit);

            Assert.True(this.Trip.AllowedSmoking);
            Assert.That(this.Trip.CountOfSeats, Is.EqualTo(1));
        }

        [Test]
        public async Task JoinTripAsyncMethodShouldWorkCorrectly()
        {
            var newUser = new ApplicationUser
            {
                Id = "TestId",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            await this.tripService.JoinTripAsync(newUser.Id, this.Trip.Id);
            var freeSeats = await this.tripService.GetCountOfFreeSeatsAsync(this.Trip.Id);

            Assert.That(freeSeats, Is.EqualTo(3));
        }

        [Test]
        public async Task LeaveTripAsyncMethodShouldWorkCorrectly()
        {
            var newUser = new ApplicationUser
            {
                Id = "TestId",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            await this.tripService.JoinTripAsync(newUser.Id, this.Trip.Id);

            await this.tripService.LeaveTripAsync(newUser.Id, this.Trip.Id);
            var freeSeats = await this.tripService.GetCountOfFreeSeatsAsync(this.Trip.Id);

            Assert.That(freeSeats, Is.EqualTo(4));
        }
    }
}