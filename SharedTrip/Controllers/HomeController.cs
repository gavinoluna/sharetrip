using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Home;
using SharedTrip.Models;
using System.Diagnostics;

namespace SharedTrip.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly ITripService tripService;

        public HomeController(
            IUserService userService,
            ITripService tripService)
        {
            this.userService = userService;
            this.tripService = tripService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                CountOfTrips = await this.tripService.GetCountOfTripsAsync(),
                CountOfUsers = await this.userService.GetCountOfUsersAsync()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}