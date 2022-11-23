using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Trip;
using SharedTrip.Core.Models.Trip.ServiceModels;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    public class TripController : BaseController
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
        public async Task<IActionResult> All([FromQuery] AllTripsQueryModel query)
        {
            var result = await this.tripService.AllAsync(
                query.StartDestinationId,
                query.EndDestinationId,
                query.Date,
                query.CurrentPage,
                AllTripsQueryModel.TripsPerPage);

            var destinations = await this.tripService.GetPopulatedPlacesAsync();

            query.StartDestinations = destinations;
            query.EndDestinations = destinations;

            query.TotalTripsCount = result.TotalTripsCount;
            query.Trips = result.Trips;

            return View(query);
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

            await PopulateTripModel(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateTripModel(model);

                return View(model);
            }

            if (await this.tripService.CheckWhetherUserIsFree(User.Id(), model.Date) == false)
            {
                this.notyfService.Error("You already have an arranged trip for that date");

                await PopulateTripModel(model);

                return View(model);
            }

            var tripId = await this.tripService.CreateTripAsync(model, User.Id());

            if (tripId == -1)
            {
                this.notyfService.Error("Something went wrong");

                await PopulateTripModel(model);

                return View(model);
            }

            return RedirectToAction(nameof(Details), new { tripId = tripId });
        }

        [HttpGet]
        public async Task<IActionResult> MyTrips([FromQuery]MyTripQueryModel query)
        {
            var result = await this.tripService.GetMyTripsAsync(User.Id(), query.CurrentPage, MyTripQueryModel.TripsPerPage);

            query.Trips = result.Trips;
            query.TotalTripsCount = result.TotalTripsCount;

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int tripId)
        {
            if ((await this.tripService.TripExists(tripId)) == false)
            {
                this.notyfService.Error("This trip does not exist");
                return RedirectToAction(nameof(MyTrips));
            }

            if ((await this.tripService.IsUserDriverOfTrip(User.Id(), tripId)) == false)
            {
                this.notyfService.Error("Only the driver of the trip can edit it");
                return RedirectToAction(nameof(Details), new { tripId = tripId });
            }

            var trip = await this.tripService.GetTripForEditAsync(tripId);

            await PopulateTripModel(trip);

            return View(trip);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateTripModel(model);

                return View(model);
            }

            if (await this.tripService.CheckWhetherUserIsFree(User.Id(), model.Date, model.Id) == false)
            {
                this.notyfService.Error("You already have an arranged trip for that date");

                await PopulateTripModel(model);

                return View(model);
            }

            var isEdited = await this.tripService.EditTripAsync(model);

            if (isEdited == false)
            {
                this.notyfService.Error("Something went wrong");

                await PopulateTripModel(model);

                return View(model);
            }

            this.notyfService.Success("The trip was edited successfully");
            return RedirectToAction(nameof(Details), new { tripId = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int tripId)
        {
            if ((await this.tripService.TripExists(tripId)) == false)
            {
                this.notyfService.Error("This trip does not exist.");
                return RedirectToAction(nameof(All));
            }

            var trip = await this.tripService.GetTripDetailsAsync(tripId);

            var detailsModel = new TripDetailsViewModel
            {
                Trip = trip,
                Car = await this.carService.GetCarAsync(trip.CarId),
                Driver = await this.userService.GetTripDriverAsync(trip.DriverId)
            };

            return View(detailsModel);
        }

        public async Task<IActionResult> JoinTrip(int tripId)
        {
            if ((await this.tripService.TripExists(tripId)) == false)
            {
                this.notyfService.Error("This trip does not exist");
                return RedirectToAction(nameof(All));
            }

            var trip = await this.tripService.GetTripForEditAsync(tripId);

            if (await this.tripService.CheckIfUserIsInTripAsync(User.Id(), trip.Id) == true)
            {
                this.notyfService.Information("You are already participating in this trip");
                return RedirectToAction(nameof(Details), new { tripId = tripId });
            }

            if (await this.tripService.CheckWhetherUserIsFree(User.Id(), trip.Date) == false)
            {
                this.notyfService.Error("You already have an arranged trip for this date");
                return RedirectToAction(nameof(Details), new { tripId = tripId });
            }

            var hasJoined = await this.tripService.JoinTripAsync(User.Id(), tripId);

            if (hasJoined == false)
            {
                this.notyfService.Error("Something went wrong");
                return RedirectToAction(nameof(Details), new { tripId = tripId });
            }

            this.notyfService.Success("You have joined the trip successfully");
            return RedirectToAction(nameof(Details), new { tripId = tripId });
        }

        public async Task<IActionResult> Delete(int tripId)
        {
            if ((await this.tripService.IsUserDriverOfTrip(User.Id(), tripId)) == false)
            {
                this.notyfService.Error("Only the driver of the trip can delete it");
                return RedirectToAction(nameof(Details), new { tripId = tripId });
            }

            if ((await this.tripService.TripExists(tripId)) == false)
            {
                this.notyfService.Error("This trip does not exist");
                return RedirectToAction(nameof(MyTrips));
            }

            var isDeleted = await this.tripService.DeleteTripAsync(tripId);

            if (isDeleted == false)
            {
                this.notyfService.Error("Something went wrong");
            }
            else
            {
                this.notyfService.Success("The trip was deleted successfully");
            }

            return RedirectToAction(nameof(MyTrips));
        }

        private async Task PopulateTripModel(ITrip model)
        {
            var destinations = await this.tripService.GetPopulatedPlacesAsync();

            model.Cars = await this.carService.GetCarsForTripAsync(User.Id());
            model.StartDestinations = destinations;
            model.EndDestinations = destinations;
        }
    }
}