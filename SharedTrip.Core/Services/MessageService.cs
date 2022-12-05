using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Chat;
using SharedTrip.Core.Models.Message;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext context;

        public MessageService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateMessage(SendMessageViewModel model)
        {
            var message = new Message
            {
                SenderId = model.SenderId,
                ReceiverId = model.ReceiverId,
                Content = model.Message,
                CreatedOn = DateTime.UtcNow
            };

            await this.context.Messages.AddAsync(message);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageViewModel>> GetMessages(string receiverId, string currentUserId)
        {
            var messages = await this.context
                .Messages
                .Where(m => (m.ReceiverId == receiverId && m.SenderId == currentUserId)
                || (m.ReceiverId == currentUserId && m.SenderId == receiverId))
                .OrderBy(m => m.CreatedOn)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    ReceiverId = m.ReceiverId,
                    SenderId = m.SenderId,
                    Content= m.Content,
                    CreatedOn = m.CreatedOn.ToString("MM/dd/yy H:mm")
                })
                .ToListAsync();

            return messages;
        }
    }
}