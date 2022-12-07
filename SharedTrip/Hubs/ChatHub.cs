using Ganss.Xss;
using Microsoft.AspNetCore.SignalR;
using SharedTrip.Core.Models.Message;

namespace SharedTrip.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(SendMessageViewModel model)
        {
            var sanitizer = new HtmlSanitizer();
            var message = sanitizer.Sanitize(model.Message);

            if (string.IsNullOrEmpty(message) ||
                string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            await this.Clients
                .User(model.ReceiverId)
                .SendAsync("ReceiveMessage", message);

            await this.Clients
                .Caller
                .SendAsync("SendMessage", message);
        }
    }
}