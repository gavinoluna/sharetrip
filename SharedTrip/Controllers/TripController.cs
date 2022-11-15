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

            return RedirectToAction(nameof(MyTrips));
        }

        [HttpGet]
        public async Task<IActionResult> MyTrips()
        {
            var trips = await this.tripService.GetMyTripsAsync(User.Id());

            ViewBag.CurrentUserId = User.Id();

            return View(trips);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int tripId)
        {
            var trip = await this.tripService.GetTripDetailsAsync(tripId);

            if (trip == null)
            {
                this.notyfService.Error("This trip does not exist.");
                return RedirectToAction(nameof(MyTrips)); //fix when all trips view is ready
            }

            var detailsModel = new TripDetailsViewModel
            {
                Trip = trip,
                Car = await this.carService.GetCarAsync(trip.CarId),
                Driver = await this.userService.GetTripDriverAsync(trip.DriverId)
            };

            return View(detailsModel);
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