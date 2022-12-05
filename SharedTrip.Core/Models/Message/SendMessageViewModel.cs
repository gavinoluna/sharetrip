using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Core.Models.Message
{
    public class SendMessageViewModel
    {
        public string ReceiverId { get; set; } = null!;

        public string SenderId { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}