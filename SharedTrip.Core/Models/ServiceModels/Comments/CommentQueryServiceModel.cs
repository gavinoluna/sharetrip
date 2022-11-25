namespace SharedTrip.Core.Models.Comments
{
    public class CommentQueryServiceModel
    {
        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}