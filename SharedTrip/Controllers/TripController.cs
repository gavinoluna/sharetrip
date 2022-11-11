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

        public TripController(
            ITripService tripService,
            ICarService carService)
        {
            this.tripService = tripService;
            this.carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var destinations = await this.tripService.GetPopulatedPlacesAsync();

            return View(new CreateTripViewModel
            {
                Cars = await this.carService.GetCarsForTripAsync(User.Id()),
                StartDestinations = destinations,
                EndDestinations = destinations
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTripViewModel model)
        {
            return View();
        }
    }
}