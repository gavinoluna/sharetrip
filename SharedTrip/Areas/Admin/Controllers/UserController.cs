using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.ServiceModels.User;
using SharedTrip.Core.Models.User;

namespace SharedTrip.Areas.Admin.Controllers
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

        public async Task<IActionResult> All([FromQuery] AllUsersQueryModel query)
        {
            try
            {
                var result = await this.userService.GetUsers(query.CurrentPage, AllUsersQueryModel.UsersPerPage);

                query.Users = result.Users;
                query.TotalUsersCount = result.TotalUsersCount;

                return View(query);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            try
            {
                var user = await this.userService.GetUserForEditAsync(userId);

                if (user == null)
                {
                    this.notyfService.Error("The user does not exist");
                    return RedirectToAction(nameof(All));
                }

                return View(user);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                return RedirectToAction(nameof(All));
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
                return RedirectToAction(nameof(All));
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