using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Models.ServiceModels.Car;
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
            try
            {
                return View(new CreateCarViewModel
                {
                    Brands = await this.carService.GetBrandsAsync(),
                    Colours = await this.carService.GetColoursAsync()
                });
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //Add logging
                return View("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarViewModel carModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateCarModel(carModel);
                    return View(carModel);
                }

                var carId = await this.carService.CreateCarAsync(carModel, User.Id());

                this.notyfService.Success("You have successfully created a car.");
                return RedirectToAction(nameof(Details), new { carId });
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //Add logging
                await PopulateCarModel(carModel);
                return View(carModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int carId)
        {
            try
            {
                var car = await this.carService.GetCarAsync(carId);

                if (car == null)
                {
                    this.notyfService.Warning("This car could not be found");
                    return RedirectToAction("Details", "User");
                }

                if (await this.carService.IsUserOwnerOfACar(User.Id(), carId) == false && User.IsInRole("Administrator") == false)
                {
                    this.notyfService.Error("You have no access to cars that are not yours");
                    return RedirectToAction(nameof(MyCars));
                }

                return View(car);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //Add logging
                return RedirectToAction(nameof(MyCars));
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyCars([FromQuery] MyCarsQueryModel query)
        {
            try
            {
                var result = await this.carService.GetMyCarsAsync(User.Id(), query.CurrentPage, MyCarsQueryModel.CarsPerPage);

                query.Cars = result.Cars;
                query.TotalCarsCount = result.TotalCarsCount;

                return View(query);
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //Add logging
                return RedirectToAction("Details", "User");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int carId)
        {
            try
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
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //Add logging
                return RedirectToAction(nameof(MyCars));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCarViewModel carModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateCarModel(carModel);
                    return View(carModel);
                }

                await this.carService.EditCarAsync(carModel);

                this.notyfService.Success("The car is edited succseffully");
                return RedirectToAction(nameof(Details), new { carId = carModel.Id });
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //Add logging
                await PopulateCarModel(carModel);
                return View(carModel);
            }
        }

        public async Task<IActionResult> Delete(int carId)
        {
            try
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

                await this.carService.DeleteAsync(carId);

                this.notyfService.Success("The car is deleted successfully");
                return RedirectToAction(nameof(MyCars));
            }
            catch (Exception)
            {
                this.notyfService.Error("Something went wrong");
                //add logging
                return RedirectToAction(nameof(MyCars));
            }
        }

        private async Task PopulateCarModel(ICarModel model)
        {
            model.Brands = await this.carService.GetBrandsAsync();
            model.Colours = await this.carService.GetColoursAsync();
        }
    }
}