using SharedTrip.Core.Models.Chat;
using SharedTrip.Core.Models.Message;

namespace SharedTrip.Core.Contracts
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageViewModel>> GetMessages(string receiverId, string currentUserId);

        Task CreateMessage(SendMessageViewModel model);
    }
}