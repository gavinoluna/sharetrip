using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly ICarService carService;

        public CarController(ICarService carService)
        {
            this.carService = carService;
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

                carModel.Brands = await this.carService.GetBrandsAsync();
                carModel.Colours = await this.carService.GetColoursAsync();
                return View(carModel);
            }

            return RedirectToAction(nameof(Details), new { carId = carId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int carId)
        {
            var car = await this.carService.GetCarAsync(carId);

            if (car == null || car.DriverId != User.Id())
            {
                return RedirectToAction("Details", "User");
            }

            return View(car);
        }
    }
}