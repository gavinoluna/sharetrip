using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.Comment;

namespace SharedTrip.Core.Models.Comments
{
    public class AddCommentViewModel
    {
        [Required]
        public string CreatorId { get; set; } = null!;

        [Required]
        public string ReceiverId { get; set; } = null!;

        [Required]
        public string CreatedOn { get; set; } = null!;

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "{0} must be between {2} and {1} characters")]
        public string Content { get; set; } = null!;
    }
}