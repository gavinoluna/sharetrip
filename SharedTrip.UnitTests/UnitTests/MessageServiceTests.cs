using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Message;
using SharedTrip.Core.Services;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.UnitTests.UnitTests
{
    [TestFixture]
    public class MessageServiceTests : UnitTestsBase
    {
        private IMessageService messageService;

        [SetUp]
        public void SetUp()
        {
            this.messageService = new MessageService(this.data);
        }

        [Test]
        public async Task CreateMessageShouldCreateMessagesCorrectly()
        {
            var newUser = new ApplicationUser
            {
                Id = "userTest",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            for (int i = 1; i <= 5; i++)
            {
                await this.messageService.CreateMessage(new SendMessageViewModel
                {
                     Message = "Test Message",
                     ReceiverId = newUser.Id,
                     SenderId = this.User.Id
                });
            }

            var messages = await this.messageService.GetMessages(newUser.Id, this.User.Id);

            Assert.That(messages.Count(), Is.EqualTo(5));
        }
    }
}