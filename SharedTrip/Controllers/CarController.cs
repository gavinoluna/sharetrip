using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using System.Security.Claims;

namespace SharedTrip.Controllers
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new AddCarViewModel
            {
                Brands = await this.carService.GetBrandsAsync(),
                Colours = await this.carService.GetColoursAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCarViewModel carModel)
        {
            if (!ModelState.IsValid)
            {
                carModel.Brands = await this.carService.GetBrandsAsync();
                carModel.Colours = await this.carService.GetColoursAsync();
                return View(carModel);
            }

            var carId = await this.carService.AddCarAsync(carModel, User.Id());

            if (carId == -1)
            {
                ModelState.AddModelError("", "Something went wrong");
                this.notyfService.Warning("Something went wrong");

                carModel.Brands = await this.carService.GetBrandsAsync();
                carModel.Colours = await this.carService.GetColoursAsync();
                return View(carModel);
            }

            this.notyfService.Success("You have successfully created a car.");
            return RedirectToAction(nameof(Details), new { carId = carId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int carId)
        {
            var car = await this.carService.GetCarAsync(carId);

            if (car == null)
            {
                this.notyfService.Warning("This car could not be found");
                return RedirectToAction("Details", "User");
            }

            if (car.DriverId != User.Id())
            {
                this.notyfService.Error("You have no access to cars that are not yours");
                return RedirectToAction(nameof(MyCars));
            }

            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> MyCars([FromQuery] AllCarsQueryModel query)
        {
            var result = await this.carService.GetMyCarsAsync(User.Id(), query.CurrentPage, AllCarsQueryModel.CarsPerPage);

            query.Cars = result.Cars;
            query.TotalCarsCount = result.TotalCarsCount;

            return View(query);
        }

        public async Task<IActionResult> Delete(int carId)
        {
            var isDeleted = await this.carService.DeleteAsync(carId);

            if (isDeleted == false)
            {
                this.notyfService.Error("Something went wrong");
                return RedirectToAction(nameof(MyCars));
            }

            this.notyfService.Success("The car is deleted successfully");
            return RedirectToAction(nameof(MyCars));
        }
    }
}