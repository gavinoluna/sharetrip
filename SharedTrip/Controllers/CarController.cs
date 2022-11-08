using Microsoft.AspNetCore.Mvc;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Models.Car;
using System.Security.Claims;

namespace SharedTrip.Controllers
{
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
                return View(carModel);
            }

            var isCreated = await this.carService.AddCarAsync(carModel, User.Id());

            if (!isCreated)
            {
                ModelState.AddModelError("", "Something went wrong");

                return View();
            }

            return RedirectToAction("User", "ProfileInfo"); //Change it when CarView is ready
        }
    }
}