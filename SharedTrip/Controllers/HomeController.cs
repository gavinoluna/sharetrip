using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService notyfService;

        public HomeController(
            IUserService userService,
            ITripService tripService,
            INotyfService notyfService)
        {
            this.userService = userService;
            this.tripService = tripService;
            this.notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new HomeViewModel
                {
                    CountOfTrips = await this.tripService.GetCountOfTripsAsync(),
                    CountOfUsers = await this.userService.GetCountOfUsersAsync()
                };

                return View(model);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //add logging
                return RedirectToAction("Details", "User");
            }
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