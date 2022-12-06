using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Chat;
using SharedTrip.Core.Models.Message;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;
        private readonly IUserService userService;
        private readonly INotyfService notyfService;

        public MessageController(
            IMessageService messageService,
            IUserService userService,
            INotyfService notyfService)
        {
            this.messageService = messageService;
            this.userService = userService;
            this.notyfService = notyfService;
        }

        public async Task<IActionResult> Chat(string userId)
        {
            try
            {
                var user = await this.userService
                .GetUserByIdAsync(userId);

                var chatModel = new UserChatViewModel()
                {
                    ReceiverId = user.Id,
                    ReceiverName = $"{user.FirstName} {user.LastName}",
                    ReceiverProfileImageUrl = user.ProfilePictureUrl
                };

                return View(chatModel);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                return RedirectToAction("Details", "User");
            }
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetMessages(string userId)
        {
            try
            {
                var messages = await this.messageService.GetMessages(userId, User.Id());
                
                return Json(messages);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessage([FromBody]SendMessageViewModel model)
        {
            var isCreated = false;

            try
            {
                await this.messageService.CreateMessage(model);
                isCreated = true;
            }
            catch (Exception)
            {
                isCreated = false;
            }

            return Json(isCreated);
        }
    }
}