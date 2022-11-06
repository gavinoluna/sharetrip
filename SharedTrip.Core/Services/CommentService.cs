using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Comments;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext context;

        public CommentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateComment(AddCommentViewModel model)
        {
            var isAdded = false;

            var comment = new Comment
            {
                CreatorId = model.CreatorId,
                ReceiverId = model.ReceiverId,
                Content = model.Content,
                //CreatedOn = model.CreatedOn
            };

            try
            {
                await this.context.Comments.AddAsync(comment);
                await this.context.SaveChangesAsync();
                isAdded = true;
            }
            catch (Exception)
            {
                isAdded = false;
            }

            return isAdded;
        }

        public async Task<IEnumerable<CommentViewModel>> GetAllByCommentsByUserIdAsync(string userId)
        {
            return await this.context
                .Comments
                .Where(c => c.ReceiverId == userId)
                .OrderBy(c => c.CreatedOn)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedOn = c.CreatedOn.ToString("{0:MM/dd/yy H:mm}"),
                    CreatorId = c.CreatorId,
                    CreatorName = $"{c.Creator.FirstName} {c.Creator.LastName}",
                    CreatorProfileImageUrl = c.Creator.ProfilePictureUrl
                })
                .ToListAsync();
        }
    }
}