using NUnit.Framework;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;
using SharedTrip.UnitTests.Mocks;
using System.IO;

namespace SharedTrip.UnitTests.UnitTests
{
    public class UnitTestsBase
    {
        protected ApplicationDbContext data;

        [SetUp]
        public async Task SetUpBase()
        {
            this.data = DatabaseMock.Instance;
            this.data.Database.EnsureDeleted();
            this.data.Database.EnsureCreated();
            await this.SeedDatabase();
        }

        public ApplicationUser User { get; private set; } = null!;

        public Car Car { get; private set; } = null!;

        private async Task SeedDatabase()
        {
            this.User = new ApplicationUser
            {
                Id = "TestUserId",
                Email = "testUser@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(this.User);

            var colour = new Colour
            {
                Id = 1,
                Name = "White"
            };

            await this.data.Colours.AddAsync(colour);

            var brand = new Brand
            {
                Id = 1,
                Name = "Mercedes-Benz"
            };

            await this.data.Brands.AddAsync(brand);

            this.Car = new Car
            {
                Year = 2020,
                BrandId = 1,
                Climatronic = true,
                ColourId = 1,
                CountOfSeats = 5,
                Model = "S Class",
                ImageUrl = "TestUrl",
                Id = 6,
                DriverId = "TestUserId"
            };

            await this.data.Cars.AddAsync(this.Car);

            await this.data.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public void TearDownBase()
            => this.data.Dispose();
    }
}