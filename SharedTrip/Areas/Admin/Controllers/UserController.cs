using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.ServiceModels.User;

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
    }
}