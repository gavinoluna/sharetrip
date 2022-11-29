using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Home;

namespace SharedTrip.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService userService;
        private readonly ITripService tripService;
        private readonly ICarService carService;
        private readonly ICommentService commentService;
        private readonly INotyfService notyfService;

        public HomeController(
            IUserService userService,
            ITripService tripService,
            ICarService carService,
            ICommentService commentService,
            INotyfService notyfService)
        {
            this.userService = userService;
            this.tripService = tripService;
            this.carService = carService;
            this.commentService = commentService;
            this.notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            try
            {
                model.CountOfUsers = await this.userService.GetCountOfUsersAsync();
                model.CountOfTrips = await this.tripService.GetCountOfTripsAsync();
                model.CountOfCars = await this.carService.GetCountOfCarsAsync();
                model.CountOfComments = await this.commentService.GetCountOfCommentsAsync();
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
            }

            return View(model);
        }
    }
}