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
            try
            {
                ProfileViewModel user;

                if (userId == null)
                {
                    user = await this.userService.GetProfileInfoAsync(User.Id());
                }
                else
                {
                    user = await this.userService.GetProfileInfoAsync(userId);
                }

                if (user == null)
                {
                    this.notyfService.Error("This user does not exist");
                    return RedirectToAction(nameof(Details));
                }

                return View(user);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //add logging
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var user = await this.userService.GetUserForEditAsync(User.Id());

                if (user == null)
                {
                    this.notyfService.Error("The user was not found");
                    return RedirectToAction(nameof(Details));
                }

                return View(user);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //add logging
                return RedirectToAction(nameof(Details));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await this.userService.EditUserAsync(model);

                this.notyfService.Success("Profile is edited successfully");
                return RedirectToAction(nameof(Details));
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //add logging
                return View(model);
            }
        }
    }
}