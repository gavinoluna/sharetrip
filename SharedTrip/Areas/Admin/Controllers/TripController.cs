using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Trip.ServiceModels;

namespace SharedTrip.Areas.Admin.Controllers
{
    public class TripController : BaseController
    {
        private readonly ITripService tripService;
        private readonly INotyfService notyfService;
        private readonly ILogger<TripController> logger;

        public TripController(
            ITripService tripService,
            INotyfService notyfService,
            ILogger<TripController> logger)
        {
            this.tripService = tripService;
            this.notyfService = notyfService;
            this.logger = logger;
        }

        public async Task<IActionResult> All([FromQuery]AllTripsQueryModel query)
        {
            try
            {
                var result = await this.tripService.AllAsync(
                query.StartDestinationId,
                query.EndDestinationId,
                query.Date,
                query.CurrentPage,
                AllTripsQueryModel.TripsPerPage);

                query.TotalTripsCount = result.TotalTripsCount;
                query.Trips = result.Trips;

                return View(query);
            }
            catch (Exception ex)
            {
                this.notyfService.Error("Something went wrong");
                this.logger.LogError($"{ex.GetType()}: {ex.Message}, \n {ex.StackTrace}");
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }
    }
}