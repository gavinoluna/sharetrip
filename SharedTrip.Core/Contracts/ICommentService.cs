using SharedTrip.Core.Models.Comments;

namespace SharedTrip.Core.Contracts
{
    public interface ICommentService
    {
        Task<bool> CreateComment(AddCommentViewModel model);

        Task<IEnumerable<CommentViewModel>> GetAllByCommentsByUserIdAsync(string userId);
    }
}