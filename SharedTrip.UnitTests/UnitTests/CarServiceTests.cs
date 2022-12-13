using Microsoft.AspNetCore.Http;
using Moq;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Services;
using SharedTrip.Infrastructure.Data.Entities;
using System.Runtime.CompilerServices;

namespace SharedTrip.UnitTests.UnitTests
{
    [TestFixture]
    public class CarServiceTests : UnitTestsBase
    {
        private ICarService carService;
        private ICloudinaryService cloudinaryService;

        [SetUp]
        public void SetUp()
        {
            this.cloudinaryService = new Mock<ICloudinaryService>().Object;
            this.carService = new CarService(this.data, this.cloudinaryService);
        }

        [Test]
        public async Task CreateCarAsyncMethodShouldCreateCarCorrectly()
        {
            var fileMock = new Mock<IFormFile>();
            var cloudinaryMock = new Mock<ICloudinaryService>();
            cloudinaryMock.Setup(x => x.UploadPicture(fileMock.Object))
            .Returns(Task.FromResult("TestUrl"));
            this.carService = new CarService(this.data, cloudinaryMock.Object);

            var carModel = new CreateCarViewModel()
            {
                Year = 2020,
                BrandId = 1,
                Climatronic = true,
                ColourId = 1,
                CountOfSeats = 5,
                Model = "S Class",
                Image = fileMock.Object
            };

            var carId = await this.carService.CreateCarAsync(carModel, this.User.Id);
            var car = await this.carService.GetCarAsync(carId);

            Assert.NotZero(carId);
            Assert.IsNotNull(car);
        }

        [Test]
        public async Task DeleteAsyncMethodShouldDeteleCarSuccessfully()
        {
            await this.carService.DeleteAsync(this.Car.Id);
            var car = await this.carService.GetCarAsync(this.Car.Id);

            Assert.IsNull(car);
        }

        [Test]
        public async Task IsOwnerOfACarMethodShouldReturnTrueIfUserIsOwner()
        {
            var isOwner = await this.carService.IsUserOwnerOfACar(this.User.Id, this.Car.Id);

            Assert.True(isOwner);
        }

        [Test]
        public async Task IsOwnerOfACarMethodShouldReturnFalseIfUserIsNotOwner()
        {
            var isOwner = await this.carService.IsUserOwnerOfACar("testId", this.Car.Id);

            Assert.That(isOwner, Is.False);
        }

        [Test]
        public async Task GetMyCarsAsyncMethodShouldReturnCarsCorrectly()
        {
            var cars = await this.carService.GetMyCarsAsync(this.User.Id);

            Assert.That(cars.Cars.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetMyCarsAsyncMethodShouldReturnCarsCorrectlyWhenUserHasNoCars()
        {
            var user = new ApplicationUser
            {
                Id = "idTest",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Test",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(user);
            await this.data.SaveChangesAsync();

            var cars = await this.carService.GetMyCarsAsync(user.Id);

            Assert.That(cars.Cars.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetCountOfCarsAsyncShouldWorkCorrectly()
        {
            var count = await this.carService.GetCountOfCarsAsync();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetCountOfCarsAsyncShouldReturnZeroWhenThereAreNoCars()
        {
            await this.carService.DeleteAsync(this.Car.Id);

            var count = await this.carService.GetCountOfCarsAsync();

            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetColoursAsyncMethodShouldReturnColoursCorrectly()
        {
            await this.data.Colours.AddRangeAsync(
                new List<Colour>()
                {
                    new Colour
                    {
                        Id = 2,
                        Name = "Black"
                    },
                    new Colour
                    {
                        Id = 3,
                        Name = "Black"
                    },
                    new Colour
                    {
                        Id = 4,
                        Name = "Black"
                    }
                });

            await this.data.SaveChangesAsync();

            var colours = await this.carService.GetColoursAsync();

            Assert.That(colours.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task GetCarsForTripAsyncMethodShouldReturnUserCarsCorrectly()
        {
            var cars = await this.carService.GetCarsForTripAsync(this.User.Id);

            Assert.That(cars.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetCarsForTripAsyncMethodShouldReturnUserCarsCorrectlyWhenThereAreDeletedCars()
        {
            await this.carService.DeleteAsync(this.Car.Id);

            var cars = await this.carService.GetCarsForTripAsync(this.User.Id);

            Assert.That(cars.Count(), Is.EqualTo(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetCarForEditAsyncShouldReturnCarCorrectly(int carId)
        {
            var car = new Car
            {
                Year = 2020,
                BrandId = 1,
                Climatronic = true,
                ColourId = 1,
                CountOfSeats = 5,
                Model = "S Class",
                ImageUrl = "TestUrl",
                Id = carId,
                DriverId = "TestUserId"
            };

            await this.data.Cars.AddAsync(car);
            await this.data.SaveChangesAsync();

            var result = await this.carService.GetCarForEditAsync(carId);

            Assert.That(result.Id, Is.EqualTo(car.Id));
        }

        [Test]
        public async Task GetBrandsAsyncMethodShouldReturnColoursCorrectly()
        {
            await this.data.Brands.AddRangeAsync(
                new List<Brand>()
                {
                    new Brand
                    {
                        Id = 2,
                        Name = "Test Brand"
                    },
                    new Brand
                    {
                        Id = 3,
                        Name = "Test Brand"
                    },
                    new Brand
                    {
                        Id = 4,
                        Name = "Test Brand"
                    }
                });

            await this.data.SaveChangesAsync();

            var brands = await this.carService.GetBrandsAsync();

            Assert.That(brands.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task GetAllCarsAsyncMethodShouldReturnCarsCorrectly()
        {
            var car = new Car
            {
                Year = 2020,
                BrandId = 1,
                Climatronic = true,
                ColourId = 1,
                CountOfSeats = 5,
                Model = "S Class",
                ImageUrl = "TestUrl",
                Id = 5,
                DriverId = "TestUserId"
            };

            await this.data.AddAsync(car);
            await this.data.SaveChangesAsync();

            var result = await this.carService.GetAllCarsAsync();

            Assert.That(2, Is.EqualTo(result.Cars.Count()));
        }

        [Test]
        public async Task EditCarAsyncMethodShouldEditCarsCorrectly()
        {
            const string newModel = "Changed Model";

            var car = await this.carService.GetCarForEditAsync(this.Car.Id);
            car.Model = newModel;

            await this.carService.EditCarAsync(car);
            var result = await this.carService.GetCarAsync(this.Car.Id);

            Assert.That(newModel, Is.EqualTo(result.Model));
        }
    }
}