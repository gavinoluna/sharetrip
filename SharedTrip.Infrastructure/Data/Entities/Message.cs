using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; } = null!;

        public ApplicationUser Sender { get; set; } = null!;

        [Required]
        public string ReceiverId { get; set; } = null!;

        public ApplicationUser Receiver { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}