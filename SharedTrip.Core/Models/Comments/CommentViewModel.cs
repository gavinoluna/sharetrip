namespace SharedTrip.Core.Models.Comments
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string CreatorId { get; set; } = null!;

        public string CreatorName { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string CreatedOn { get; set; } = null!;

        public string CreatorProfileImageUrl { get; set; } = null!;
    }
}