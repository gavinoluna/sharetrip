using Microsoft.AspNetCore.Http;
using Moq;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Services;
using SharedTrip.Infrastructure.Data.Entities;

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
            var cloudinaryMock = new Mock<ICloudinaryService>();
            this.carService = new CarService(this.data, cloudinaryMock.Object);

            await this.carService.DeleteAsync(this.Car.Id);
            var car = await this.carService.GetCarAsync(this.Car.Id);

            Assert.IsNull(car);
        }

        [Test]
        public async Task IsOwnerOfACarMethodShouldReturnTrueIfUserIsOwner()
        {
            var cloudinaryMock = new Mock<ICloudinaryService>();
            this.carService = new CarService(this.data, cloudinaryMock.Object);

            var isOwner = await this.carService.IsUserOwnerOfACar(this.User.Id, this.Car.Id);

            Assert.True(isOwner);
        }

        [Test]
        public async Task IsOwnerOfACarMethodShouldReturnFalseIfUserIsNotOwner()
        {
            var cloudinaryMock = new Mock<ICloudinaryService>();
            this.carService = new CarService(this.data, cloudinaryMock.Object);

            var isOwner = await this.carService.IsUserOwnerOfACar("testId", this.Car.Id);

            Assert.False(isOwner);
        }

        [Test]
        public async Task GetMyCarsAsyncMethodShouldReturnCarsCorrectly()
        {
            var cloudinaryMock = new Mock<ICloudinaryService>();
            this.carService = new CarService(this.data, cloudinaryMock.Object);

            var cars = await this.carService.GetMyCarsAsync(this.User.Id);

            Assert.That(cars.Cars.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetMyCarsAsyncMethodShouldReturnCarsCorrectlyWhenUserHasNoCars()
        {
            var cloudinaryMock = new Mock<ICloudinaryService>();
            this.carService = new CarService(this.data, cloudinaryMock.Object);

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
            var cloudinaryMock = new Mock<ICloudinaryService>();
            this.carService = new CarService(this.data, cloudinaryMock.Object);

            var count = await this.carService.GetCountOfCarsAsync();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetCountOfCarsAsyncShouldReturnZeroWhenThereAreNoCars()
        {
            var cloudinaryMock = new Mock<ICloudinaryService>();
            this.carService = new CarService(this.data, cloudinaryMock.Object);

            await this.carService.DeleteAsync(this.Car.Id);

            var count = await this.carService.GetCountOfCarsAsync();

            Assert.That(count, Is.EqualTo(0));
        }
    }
}