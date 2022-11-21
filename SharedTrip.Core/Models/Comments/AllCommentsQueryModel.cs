namespace SharedTrip.Core.Models.Comments
{
    public class AllCommentsQueryModel
    {
        public const int CommentsPerPage = 3;

        public string ReceiverId { get; set; } = null!;

        public int CurrentPage { get; set; } = 1;

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}