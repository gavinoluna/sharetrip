using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Comments;
using SharedTrip.Core.Services;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.UnitTests.UnitTests
{
    [TestFixture]
    public class CommentServiceTests : UnitTestsBase
    {
        private ICommentService commentService;

        [SetUp]
        public void SetUp()
        {
            this.commentService = new CommentService(this.data);
        }

        [Test]
        public async Task CreateCommentAsyncShouldCreateCommentsCorrectly()
        {
            var newComments = new List<AddCommentViewModel>()
            {
                new AddCommentViewModel
                {
                    Content = "Test",
                    CreatorId = this.User.Id,
                    ReceiverId = "testUser"
                },
                new AddCommentViewModel
                {
                    Content = "Test",
                    CreatorId = this.User.Id,
                    ReceiverId = "testUser"
                },
                new AddCommentViewModel
                {
                    Content = "Test",
                    CreatorId = this.User.Id,
                    ReceiverId = "testUser"
                },
            };

            foreach (var comment in newComments)
            {
                var result = await this.commentService.CreateComment(comment);
                Assert.True(result);
            }

            var countOfComments = await this.commentService.GetCountOfCommentsAsync();

            Assert.That(countOfComments, Is.EqualTo(3));
        }

        [Test]
        public async Task CreateCommentAsyncShouldNotCreateCommentsWhenCommentIsScipt()
        {
            var comment = new AddCommentViewModel
            {
                Content = "<script>alert(1)</script>",
                CreatorId = this.User.Id,
                ReceiverId = "testUser"
            };

            var result = await this.commentService.CreateComment(comment);
            var countOfComments = await this.commentService.GetCountOfCommentsAsync();

            Assert.False(result);
            Assert.That(countOfComments, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllCommentsByUserIdAsyncShouldReturnCommentsPerPageCorrectly()
        {
            var newUser = new ApplicationUser
            {
                Id = "testId",
                Email = "test@abv.bg",
                FirstName = "Test",
                LastName = "Testov",
                ProfilePictureUrl = "testUrl"
            };

            await this.data.Users.AddAsync(newUser);
            await this.data.SaveChangesAsync();

            for (int i = 1; i <= 7; i++)
            {
                await this.data.Comments.AddAsync(new Comment
                {
                    Id = i,
                    Content = "Test",
                    CreatedOn = DateTime.UtcNow,
                    CreatorId = this.User.Id,
                    ReceiverId = newUser.Id
                });

                await this.data.SaveChangesAsync();
            }

            var comments = await this.commentService.GetAllByCommentsByUserIdAsync(newUser.Id);

            Assert.Multiple(() =>
            {
                Assert.That(comments.Comments.Count(), Is.EqualTo(3));
                Assert.That(comments.TotalCommentsCount, Is.EqualTo(7));
            });
        }
    }
}