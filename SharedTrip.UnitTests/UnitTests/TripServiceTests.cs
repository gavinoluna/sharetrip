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
                Assert.That(tripsCount, Is.EqualTo(1));
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
    }
}