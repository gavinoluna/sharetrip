using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> MyProfile(string userId = null)
        {
            var user = await this.userService.GetMyProfileDataAsync(User.Id());

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}