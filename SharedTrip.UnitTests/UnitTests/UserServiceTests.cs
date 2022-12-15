using Moq;
using NUnit.Framework;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Services;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.UnitTests.UnitTests
{
    [TestFixture]
    public class UserServiceTests : UnitTestsBase
    {
        private IUserService userService;
        private ICloudinaryService cloudinaryService;

        [SetUp]
        public void SetUp()
        {
            this.cloudinaryService = new Mock<ICloudinaryService>().Object;
            this.userService = new UserService(this.data, this.cloudinaryService); 
        }

        [Test]
        public async Task HasCarMethodShouldReturnTrueIfUserHasCar()
        {
            var hasCar = await this.userService.HasCar(this.User.Id);

            Assert.True(hasCar);
        }

        [Test]
        public async Task HasCarMethodShouldReturnFalseIfUserHasNotCar()
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

            var hasCar = await this.userService.HasCar(newUser.Id);

            Assert.False(hasCar);
        }
    }
}