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

        public TripController(
            ITripService tripService,
            INotyfService notyfService)
        {
            this.tripService = tripService;
            this.notyfService = notyfService;
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
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //add logging
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }
    }
}