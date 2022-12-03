using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.ServiceModels.Car;
using System.Security.Claims;

namespace SharedTrip.Areas.Admin.Controllers
{
    public class CarController : BaseController
    {
        private readonly ICarService carService;
        private readonly INotyfService notyfService;

        public CarController(
            ICarService carService,
            INotyfService notyfService)
        {
            this.carService = carService;
            this.notyfService = notyfService;
        }

        public async Task<IActionResult> All([FromQuery] AllCarsQueryModel query)
        {
            try
            {
                var result = await this.carService.GetAllCarsAsync(query.CurrentPage, AllCarsQueryModel.CarsPerPage);

                query.Cars = result.Cars;
                query.TotalCarsCount = result.TotalCarsCount;

                return View(query);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //Add logging
                return RedirectToAction("Index", "Home");
            }
        }
    }
}