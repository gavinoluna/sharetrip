using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Trip;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripService tripService;
        private readonly ICarService carService;
        private readonly IUserService userService;
        private readonly INotyfService notyfService;

        public TripController(
            ITripService tripService,
            ICarService carService,
            IUserService userService,
            INotyfService notyfService)
        {
            this.tripService = tripService;
            this.carService = carService;
            this.userService = userService;
            this.notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!await this.userService.HasCar(User.Id()))
            {
                this.notyfService.Information("You have to posses a car to create a trip.");
                return RedirectToAction("Create", "Car");
            }

            var model = new CreateTripViewModel();

            await PopulateCreateTripViewModel(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCreateTripViewModel(model);

                return View(model);
            }

            if (await this.tripService.CheckWhetherUserIsFree(User.Id(), model) == false)
            {
                this.notyfService.Error("You already have an arranged trip for that date");

                await PopulateCreateTripViewModel(model);

                return View(model);
            }

            var tripId = await this.tripService.CreateTripAsync(model, User.Id());

            if (tripId == -1)
            {
                this.notyfService.Error("Something went wrong");

                await PopulateCreateTripViewModel(model);

                return View(model);
            }

            return View("Details", new { tripId = tripId });
        }

        private async Task PopulateCreateTripViewModel(CreateTripViewModel model)
        {
            var destinations = await this.tripService.GetPopulatedPlacesAsync();

            model.Cars = await this.carService.GetCarsForTripAsync(User.Id());
            model.StartDestinations = destinations;
            model.EndDestinations = destinations;
        }
    }
}