using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        private readonly ILogger<HomeController> logger;

        public HomeController(
            IUserService userService,
            ITripService tripService,
            INotyfService notyfService,
            ILogger<HomeController> logger)
        {
            this.userService = userService;
            this.tripService = tripService;
            this.notyfService = notyfService;
            this.logger = logger;
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
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction("Details", "User");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404)
                {
                    var viewName = statusCode.ToString();
                    return View(viewName);
                }
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}