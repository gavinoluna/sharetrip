namespace SharedTrip.Core.Models.Chat
{
    public class UserChatViewModel
    {
        public string ReceiverId { get; set; } = null!;

        public string ReceiverName { get; set; } = null!;

        public string ReceiverProfileImageUrl { get; set; } = null!;
    }
}