namespace SharedTrip.Core.Models.Chat
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public string CreatedOn { get; set; } = null!;

        public string SenderId { get; set; } = null!;

        public string ReceiverId { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}