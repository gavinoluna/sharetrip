using SharedTrip.Core.Models.Comments;

namespace SharedTrip.Core.Contracts
{
    public interface ICommentService
    {
        Task<bool> CreateComment(AddCommentViewModel model);

        Task<CommentQueryServiceModel> GetAllByCommentsByUserIdAsync(string receiverId, int currentPage = 1, int commentsPerPage = 3);

        Task<int> GetCountOfCommentsAsync();
    }
}