using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Models.ServiceModels.Car;
using SharedTrip.Core.Models.Trip;
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
            return View(new CreateCarViewModel
            {
                Brands = await this.carService.GetBrandsAsync(),
                Colours = await this.carService.GetColoursAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarViewModel carModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCarModel(carModel);
                return View(carModel);
            }

            var carId = await this.carService.AddCarAsync(carModel, User.Id());

            if (carId == -1)
            {
                this.notyfService.Warning("Something went wrong");

                await PopulateCarModel(carModel);
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

        [HttpGet]
        public async Task<IActionResult> Edit(int carId)
        {
            var car = await this.carService.GetCarForEditAsync(carId);

            if (car == null)
            {
                this.notyfService.Error("This car does not exist");
                return RedirectToAction(nameof(MyCars));
            }

            if (await this.carService.IsUserOwnerOfACar(User.Id(), carId) == false)
            {
                this.notyfService.Error("Only the owner of the car can edit it");
                return RedirectToAction(nameof(MyCars));
            }

            await PopulateCarModel(car);

            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCarViewModel carModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCarModel(carModel);
                return View(carModel);
            }

            var isEdited = await this.carService.EditCarAsync(carModel);

            if (isEdited == false)
            {
                this.notyfService.Error("Something went wrong");
                await PopulateCarModel(carModel);
                return View(carModel);
            }

            this.notyfService.Success("The car is edited succseffully");
            return RedirectToAction(nameof(Details), new { carId = carModel.Id });
        }

        public async Task<IActionResult> Delete(int carId)
        {
            var car = await this.carService.GetCarAsync(carId);

            if (car == null)
            {
                this.notyfService.Error("This car does not exist");
                return RedirectToAction(nameof(MyCars));
            }

            if (await this.carService.IsUserOwnerOfACar(User.Id(), car.Id) == false)
            {
                this.notyfService.Error("Only the owner of a car can delete it");
                return RedirectToAction(nameof(MyCars));
            }

            var isDeleted = await this.carService.DeleteAsync(carId);

            if (isDeleted == false)
            {
                this.notyfService.Error("Something went wrong");
                return RedirectToAction(nameof(MyCars));
            }

            this.notyfService.Success("The car is deleted successfully");
            return RedirectToAction(nameof(MyCars));
        }

        private async Task PopulateCarModel(ICarModel model)
        {
            model.Brands = await this.carService.GetBrandsAsync();
            model.Colours = await this.carService.GetColoursAsync();
        }
    }
}