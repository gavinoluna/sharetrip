using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.User;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly INotyfService notyfService;

        public UserController(
            IUserService userService,
            INotyfService notyfService)
        {
            this.userService = userService;
            this.notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string userId = null)
        {
            ProfileViewModel user;

            if (userId == null)
            {
                user = await this.userService.GetProfileInfoAsync(User.Id(), true);
            }
            else
            {
                user = await this.userService.GetProfileInfoAsync(userId, false);
            }

            if (user == null)
            {
                this.notyfService.Error("This user does not exist");
                return RedirectToAction(nameof(Details));
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await this.userService.GetUserForEditAsync(User.Id());

            if (user == null)
            {
                this.notyfService.Error("The user was not found");
                return RedirectToAction(nameof(Details));
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isEdited = await this.userService.EditUserAsync(model);

            if (isEdited == false)
            {
                this.notyfService.Error("Something went wrong");
                return View(model);
            }

            this.notyfService.Success("Profile is edited successfully");
            return RedirectToAction(nameof(Details));
        }
    }
}