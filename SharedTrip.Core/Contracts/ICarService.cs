﻿using SharedTrip.Core.Models.Car;

namespace SharedTrip.Core.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<ProfileCarViewModel>> GetMyCarsAsync(string userId);

        Task<int> AddCarAsync(AddCarViewModel model, string userId);

        Task<IEnumerable<BrandViewModel>> GetBrandsAsync();

        Task<IEnumerable<ColourViewModel>> GetColoursAsync();

        Task<CarDetailsViewModel> GetCarAsync(int carId);

        Task<IEnumerable<CreateTripCarViewModel>> GetCarsForTripAsync(string userId);

        Task<CarQueryServiceModel> GetMyCarsAsync(string userId, int currentPage = 1, int carsPerPage = 5);
    }
}