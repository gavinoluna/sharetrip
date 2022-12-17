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
        private readonly ILogger<TripController> logger;    

        public TripController(
            ITripService tripService,
            ICarService carService,
            IUserService userService,
            INotyfService notyfService,
            ILogger<TripController> logger)
        {
            this.tripService = tripService;
            this.carService = carService;
            this.userService = userService;
            this.notyfService = notyfService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllTripsQueryModel query)
        {
            try
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
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
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
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTripViewModel model)
        {
            try
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

                return RedirectToAction(nameof(Details), new { tripId });
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                await PopulateTripModel(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyTrips([FromQuery] MyTripQueryModel query)
        {
            try
            {
                var result = await this.tripService.GetMyTripsAsync(User.Id(), query.CurrentPage, MyTripQueryModel.TripsPerPage);

                query.Trips = result.Trips;
                query.TotalTripsCount = result.TotalTripsCount;

                return View(query);
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int tripId)
        {
            try
            {
                if ((await this.tripService.TripExists(tripId)) == false)
                {
                    this.notyfService.Error("This trip does not exist");
                    return RedirectToAction(nameof(MyTrips));
                }

                if ((await this.tripService.IsUserDriverOfTrip(User.Id(), tripId)) == false)
                {
                    this.notyfService.Error("Only the driver of the trip can edit it");
                    return RedirectToAction(nameof(Details), new { tripId });
                }

                var trip = await this.tripService.GetTripForEditAsync(tripId);

                await PopulateTripModel(trip);

                return View(trip);
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction(nameof(MyTrips));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTripViewModel model)
        {
            try
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

                await this.tripService.EditTripAsync(model);

                this.notyfService.Success("The trip was edited successfully");
                return RedirectToAction(nameof(Details), new { tripId = model.Id });
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                await PopulateTripModel(model);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int tripId)
        {
            try
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
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction(nameof(All));
            }
        }

        public async Task<IActionResult> JoinTrip(int tripId)
        {
            try
            {
                if ((await this.tripService.TripExists(tripId)) == false)
                {
                    this.notyfService.Error("This trip does not exist");
                    return RedirectToAction(nameof(All));
                }

                var trip = await this.tripService.GetTripForEditAsync(tripId);

                if (await this.tripService.CheckIfUserIsInTripAsync(User.Id(), trip.Id) == true) // Move before CheckWhetherUserIsFree
                {
                    this.notyfService.Information("You are already participating in this trip");
                    return RedirectToAction(nameof(Details), new { tripId });
                }

                if (await this.tripService.CheckWhetherUserIsFree(User.Id(), trip.Date) == false)
                {
                    this.notyfService.Error("You already have an arranged trip for this date");
                    return RedirectToAction(nameof(Details), new { tripId });
                }

                if (await this.tripService.GetCountOfFreeSeatsAsync(trip.Id) <= 0)
                {
                    this.notyfService.Error("There are no free seats in this trip");
                    return RedirectToAction(nameof(Details), new { tripId });
                }

                await this.tripService.JoinTripAsync(User.Id(), tripId);

                this.notyfService.Success("You have joined the trip successfully");
                return RedirectToAction(nameof(Details), new { tripId });
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction(nameof(Details), new { tripId });
            }
        }

        public async Task<IActionResult> LeaveTrip(int tripId)
        {
            try
            {
                if ((await this.tripService.TripExists(tripId)) == false)
                {
                    this.notyfService.Error("This trip does not exist");
                    return RedirectToAction(nameof(MyTrips));
                }

                var trip = await this.tripService.GetTripForEditAsync(tripId);

                if (await this.tripService.CheckIfUserIsInTripAsync(User.Id(), trip.Id) == false)
                {
                    this.notyfService.Error("You are not participating in this trip");
                    return RedirectToAction(nameof(Details), new { tripId });
                }

                if (User.Id() == trip.DriverId)
                {
                    this.notyfService.Error("You cannot leave your own trip.");
                    return RedirectToAction(nameof(Details), new { tripId });
                }

                await this.tripService.LeaveTripAsync(User.Id(), tripId);

                this.notyfService.Success("You have left the trip successfully");
                return RedirectToAction(nameof(Details), new { tripId });
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction(nameof(Details), new { tripId });
            }
        }

        public async Task<IActionResult> Delete(int tripId)
        {
            try
            {
                if ((await this.tripService.TripExists(tripId)) == false)
                {
                    this.notyfService.Error("This trip does not exist");
                    return RedirectToAction(nameof(MyTrips));
                }

                if ((await this.tripService.IsUserDriverOfTrip(User.Id(), tripId)) == false)
                {
                    this.notyfService.Error("Only the driver of the trip can delete it");
                    return RedirectToAction(nameof(Details), new { tripId });
                }

                await this.tripService.DeleteTripAsync(tripId);

                this.notyfService.Success("The trip was deleted successfully");
                return RedirectToAction(nameof(MyTrips));
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"Error: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction(nameof(MyTrips));
            }
        }

        private async Task PopulateTripModel(ITripModel model)
        {
            var destinations = await this.tripService.GetPopulatedPlacesAsync();

            model.Cars = await this.carService.GetCarsForTripAsync(User.Id());
            model.StartDestinations = destinations;
            model.EndDestinations = destinations;
        }
    }
}