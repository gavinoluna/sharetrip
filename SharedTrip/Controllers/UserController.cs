using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.User;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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
                return NotFound();
            }

            return View(user);
        }
    }
}